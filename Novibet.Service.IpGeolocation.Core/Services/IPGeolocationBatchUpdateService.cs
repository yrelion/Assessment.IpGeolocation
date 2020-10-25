using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Novibet.Service.IpGeolocation.Common.Models;
using Novibet.Service.IpGeolocation.Core.Models;

namespace Novibet.Service.IpGeolocation.Core.Services
{
    public delegate void IPGeolocationProcessor(IEnumerable<IPGeolocationUpdateRequest> request);

    public class IPGeolocationBatchUpdateService : BackgroundService
    {
        /// <summary>
        /// The worker configuration options
        /// </summary>
        private readonly IPGeolocationHostedWorkerSettings _settings;

        /// <summary>
        /// The completed <see cref="GeolocationBatchUpdateJob"/> registry
        /// </summary>
        private ObservableCollection<GeolocationBatchUpdateJob> CompletedJobs { get; }

        /// <summary>
        /// The read-only collection of completed <see cref="GeolocationBatchUpdateJob"/>s
        /// reflecting <see cref="CompletedJobs"/>
        /// </summary>
        private ReadOnlyCollection<GeolocationBatchUpdateJob> CompletedJobsReadOnly { get; }

        /// <summary>
        /// The executing <see cref="GeolocationBatchUpdateJob"/> collection where
        /// jobs are placed while their execution is initiated
        /// </summary>
        private ObservableCollection<GeolocationBatchUpdateJob> ExecutingJobs { get; }

        /// <summary>
        /// The pending <see cref="GeolocationBatchUpdateJob"/> collection where
        /// jobs are placed while their executing is pending
        /// </summary>
        private ObservableCollection<GeolocationBatchUpdateJob> PendingJobs { get; }

        /// <summary>
        /// The <see cref="IPGeolocationUpdateRequest"/> buffer per
        /// each executing <see cref="GeolocationBatchUpdateJob"/>'s <see cref="Guid"/>
        /// </summary>
        /// <remarks>
        /// Buffered items are subject to retrieval based on the <see cref="JobItemBatchSize"/>
        /// and are removed upon being processed
        /// </remarks>
        private Dictionary<Guid, List<IPGeolocationUpdateRequest>> Buffer { get; }

        /// <summary>
        /// The <see cref="GeolocationBatchUpdateJob"/> size
        /// </summary>
        public int JobBatchSize => _settings.JobBatchSize;

        /// <summary>
        /// The <see cref="IPGeolocationUpdateRequest"/> size to consume
        /// from the <see cref="Buffer"/>
        /// </summary>
        public int JobItemBatchSize => _settings.JobItemBatchSize;

        /// <summary>
        /// The worker execution interval in milliseconds
        /// </summary>
        /// <remarks>
        /// Zero value interval will block the main thread and should therefore be avoided
        /// </remarks>
        public int Interval => _settings.Interval == 0 ? 1 : _settings.Interval;

        /// <summary>
        /// The available positions for the processing of new <see cref="GeolocationBatchUpdateJob"/>s
        /// </summary>
        public int OpenJobBatchSlots => JobBatchSize - ExecutingJobs.Count;

        public IPGeolocationBatchUpdateService(IPGeolocationHostedWorkerSettings settings)
        {
            _settings = settings;

            CompletedJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            CompletedJobsReadOnly = new ReadOnlyCollection<GeolocationBatchUpdateJob>(CompletedJobs);
            ExecutingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();

            PendingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            PendingJobs.CollectionChanged += PendingJobsOnCollectionChanged;

            Buffer = new Dictionary<Guid, List<IPGeolocationUpdateRequest>>();
        }

        /// <summary>
        /// Provides long running worker execution
        /// </summary>
        /// <param name="stoppingToken">The <see cref="CancellationToken"/> to stop the worker</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TryQueuePendingJobsToExecuting();
                TryProcessExecutingJobItems();
                await Task.Delay(Interval, stoppingToken);
            }
        }

        /// <summary>
        /// Encapsulates the given <paramref name="request"/> within a <see cref="GeolocationBatchUpdateJob"/>
        /// along with the <see cref="IPGeolocationProcessor"/> delegate to eventually invoke when
        /// processing begins
        /// </summary>
        /// <param name="request">The <see cref="List{T}"/> of <see cref="IPGeolocationUpdateRequest"/></param>
        /// <param name="processor">The <see cref="IPGeolocationProcessor"/> delegate to invoke at processing time</param>
        /// <returns>
        /// The assigned <see cref="Guid"/> associated with the newly created
        /// <see cref="GeolocationBatchUpdateJob"/>
        /// </returns>
        public Guid AddJob(List<IPGeolocationUpdateRequest> request, IPGeolocationProcessor processor)
        {
            var backgroundJob = new GeolocationBatchUpdateJob(request, processor);
            PendingJobs.Add(backgroundJob);

            return backgroundJob.Id;
        }

        /// <summary>
        /// Retrieves the <see cref="BackgroundJobStatus"/> of a <see cref="GeolocationBatchUpdateJob"/>
        /// by Id. Returns null when job not found.
        /// </summary>
        /// <param name="id">The <see cref="GeolocationBatchUpdateJob"/> identifier</param>
        /// <returns>The <see cref="BackgroundJobStatus"/> or null if not found</returns>
        public BackgroundJobStatus GetJobStatus(Guid id)
        { 
            var job = SearchJob(id);

            if (job == null)
                return null;

            return new BackgroundJobStatus
            {
                TotalItemCount = job.RequestItemCount,
                RemainingItemCount = job.RemainingItemCount
            };
        }

        /// <summary>
        /// Searches for a <see cref="GeolocationBatchUpdateJob"/> by its id in every collection.
        /// Returns null when not found.
        /// </summary>
        /// <param name="id">The <see cref="GeolocationBatchUpdateJob"/> identifier</param>
        /// <returns>The <see cref="GeolocationBatchUpdateJob"/> or null if not found</returns>
        private GeolocationBatchUpdateJob SearchJob(Guid id)
        {
            var registeredJobs = PendingJobs.Union(ExecutingJobs).Union(CompletedJobsReadOnly);
            var job = registeredJobs.FirstOrDefault(x => x.Id == id);

            return job;
        }

        /// <summary>
        /// Attempts to queue pending <see cref="GeolocationBatchUpdateJob"/>s to executing based on
        /// the available <see cref="JobBatchSize"/>
        /// </summary>
        private void TryQueuePendingJobsToExecuting()
        {
            if (ExecutingJobs.Count >= JobBatchSize)
                return; // Skip queueing when full
            
            var jobs = PendingJobs.Take(OpenJobBatchSlots);

            MovePendingToExecuting(jobs);
        }

        /// <summary>
        /// Attempts to process executing <see cref="GeolocationBatchUpdateJob"/>s'
        /// <see cref="IPGeolocationUpdateRequest"/>s based on the available <see cref="JobItemBatchSize"/>
        /// </summary>
        private void TryProcessExecutingJobItems()
        {
            if (!ExecutingJobs.Any())
                return;

            foreach (var bufferItem in Buffer)
            {
                // Retrieve buffered items by item batch size specification
                var itemsToProcess = bufferItem.Value.Take(JobItemBatchSize).ToList();

                // Retrieve matching job
                var job = ExecutingJobs.FirstOrDefault(x => x.Id == bufferItem.Key);

                if (job == null)
                    continue;

                // Invoke delegate to process the series of job items
                job.Processor.Invoke(itemsToProcess);

                // Remove processed job items from the buffer for the current job
                itemsToProcess.ForEach(x => Buffer.FirstOrDefault(i => i.Key == bufferItem.Key)
                    .Value.Remove(x));

                job.RemainingItemCount -= itemsToProcess.Count;

                if (!bufferItem.Value.Any())
                    MoveExecutingToCompleted(job);
            }
        }

        /// <summary>
        /// Moves the provided <see cref="GeolocationBatchUpdateJob"/> from pending to executing
        /// </summary>
        /// <param name="job">The <see cref="GeolocationBatchUpdateJob"/> to move</param>
        private void MovePendingToExecuting(GeolocationBatchUpdateJob job)
        {
            PendingJobs.Remove(job);
            ExecutingJobs.Add(job);
            Buffer.Add(job.Id, job.Request as List<IPGeolocationUpdateRequest>);
        }

        /// <summary>
        /// Moves the provided <see cref="GeolocationBatchUpdateJob"/> from executing to completed
        /// </summary>
        /// <param name="job">The <see cref="GeolocationBatchUpdateJob"/> to move</param>
        private void MoveExecutingToCompleted(GeolocationBatchUpdateJob job)
        {
            ExecutingJobs.Remove(job);
            CompletedJobs.Add(job);
            Buffer.Remove(job.Id);
        }

        /// <summary>
        /// Moves the provided <see cref="List{T}"/> of <see cref="GeolocationBatchUpdateJob"/>s
        /// from pending to executing
        /// </summary>
        /// <param name="jobs">The <see cref="List{T}"/> of <see cref="GeolocationBatchUpdateJob"/>s
        /// to move</param>
        private void MovePendingToExecuting(IEnumerable<GeolocationBatchUpdateJob> jobs)
            => jobs?.ToList().ForEach(MovePendingToExecuting);

        /// <summary>
        /// Moves the provided <see cref="List{T}"/> of <see cref="GeolocationBatchUpdateJob"/>s
        /// from executing to completed
        /// </summary>
        /// <param name="jobs">The <see cref="List{T}"/> of <see cref="GeolocationBatchUpdateJob"/>s
        /// to move</param>
        private void MoveExecutingToCompleted(IEnumerable<GeolocationBatchUpdateJob> jobs)
            => jobs?.ToList().ForEach(MoveExecutingToCompleted);
        
        /// <summary>
        /// Handles actions for the <see cref="PendingJobs"/> upon the
        /// <see cref="ObservableCollection{T}"/> modification
        /// </summary>
        /// <param name="sender">The event emitting object</param>
        /// <param name="e">The event data</param>
        private void PendingJobsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                TryQueuePendingJobsToExecuting();
        }
    }
}
