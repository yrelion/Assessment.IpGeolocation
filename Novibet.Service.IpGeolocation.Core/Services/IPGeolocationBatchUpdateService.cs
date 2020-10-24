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
        public ObservableCollection<GeolocationBatchUpdateJob> CompletedJobs { get; }
        public ObservableCollection<GeolocationBatchUpdateJob> ExecutingJobs { get; }
        public ObservableCollection<GeolocationBatchUpdateJob> PendingJobs { get; }
        public Dictionary<Guid, List<IPGeolocationUpdateRequest>> Buffer { get; }

        public const int JobBatchExecutionSize = 2;
        public const int ItemBatchProcessSize = 3;

        public IPGeolocationBatchUpdateService()
        {
            CompletedJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            ExecutingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            PendingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            PendingJobs.CollectionChanged += PendingJobsOnCollectionChanged;

            Buffer = new Dictionary<Guid, List<IPGeolocationUpdateRequest>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TryQueueNewJobs();
                TryProcessExecutingJobs();
                await Task.Delay(5000, stoppingToken);
            }
        }

        public Guid AddJob(List<IPGeolocationUpdateRequest> request, IPGeolocationProcessor processor)
        {
            var backgroundJob = new GeolocationBatchUpdateJob(request, processor);

            PendingJobs.Add(backgroundJob);

            return backgroundJob.Id;
        }

        public BackgroundJobStatus GetJobStatus(Guid id)
        {
            var status = new BackgroundJobStatus();

            var job = SearchJob(id);

            if (job == null)
                return null;

            status.TotalItemCount = job.RequestItemCount;
            status.RemainingItemCount = job.RemainingItemCount;

            return status;
        }

        private GeolocationBatchUpdateJob SearchJob(Guid id)
        {
            var pendingJob = PendingJobs.FirstOrDefault(x => x.Id == id);

            if (pendingJob != null)
                return pendingJob;

            var executingJob = ExecutingJobs.FirstOrDefault(x => x.Id == id);

            if (executingJob != null)
                return executingJob;

            var completedJob = CompletedJobs.FirstOrDefault(x => x.Id == id);

            return completedJob;
        }

        private void TryQueueNewJobs()
        {
            if (ExecutingJobs.Count >= JobBatchExecutionSize)
                return; // Skip queueing when full

            var availablePositions = JobBatchExecutionSize - ExecutingJobs.Count;

            var jobs = PendingJobs.Take(availablePositions).ToList();

            if (!jobs.Any())
                return;

            jobs.ForEach(x =>
            {
                ExecutingJobs.Add(x);
                Buffer.Add(x.Id, x.Request as List<IPGeolocationUpdateRequest>);
                PendingJobs.Remove(x);
            });
        }

        private void TryProcessExecutingJobs()
        {
            if (!ExecutingJobs.Any())
                return;

            foreach (var bufferItem in Buffer)
            {
                var itemsToProcess = bufferItem.Value.Take(ItemBatchProcessSize).ToList();

                // Handle selected items
                var executingJob = ExecutingJobs.FirstOrDefault(x => x.Id == bufferItem.Key);

                if(executingJob == null)
                    continue;

                executingJob.Processor.Invoke(itemsToProcess);

                // remove selected items from the buffer for specific job id
                itemsToProcess.ForEach(x => Buffer.FirstOrDefault(i => i.Key == bufferItem.Key).Value.Remove(x));

                executingJob.RemainingItemCount -= itemsToProcess.Count;

                if (!bufferItem.Value.Any())
                    FinishExecutingJob(executingJob);
            }
        }

        private void FinishExecutingJob(GeolocationBatchUpdateJob job)
        {
            CompletedJobs.Add(job);
            Buffer.Remove(job.Id);
            ExecutingJobs.Remove(job);
        }

        private void PendingJobsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                TryQueueNewJobs();
        }
    }
}
