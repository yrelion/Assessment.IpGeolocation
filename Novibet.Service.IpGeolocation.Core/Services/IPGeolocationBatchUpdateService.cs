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
        public ObservableCollection<GeolocationBatchUpdateJob> ExecutingJobs { get; }
        public ObservableCollection<GeolocationBatchUpdateJob> PendingJobs { get; }
        public Dictionary<Guid, List<IPGeolocationUpdateRequest>> ExecutingJobItems { get; }

        public const int JobBatchExecutionSize = 2;
        public const int ItemBatchProcessSize = 3;

        public IPGeolocationBatchUpdateService()
        {
            ExecutingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            PendingJobs = new ObservableCollection<GeolocationBatchUpdateJob>();
            PendingJobs.CollectionChanged += PendingJobsOnCollectionChanged;

            ExecutingJobItems = new Dictionary<Guid, List<IPGeolocationUpdateRequest>>();
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

        public async Task<BackgroundJobStatusType> GetJobStatus(Guid id)
        {
            if (PendingJobs.Any(x => x.Id == id))
                return BackgroundJobStatusType.Pending;

            if (ExecutingJobs.Any(x => x.Id == id))
                return BackgroundJobStatusType.Processing;

            return BackgroundJobStatusType.Completed;
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
                ExecutingJobItems.Add(x.Id, x.Request as List<IPGeolocationUpdateRequest>);
                PendingJobs.Remove(x);
            });
        }

        private void TryProcessExecutingJobs()
        {
            if (!ExecutingJobs.Any())
                return;

            foreach (var jobItem in ExecutingJobItems)
            {
                var itemsToProcess = jobItem.Value.Take(ItemBatchProcessSize).ToList();

                // Handle selected items
                var executingJob = ExecutingJobs.FirstOrDefault(x => x.Id == jobItem.Key);

                if(executingJob == null)
                    continue;

                executingJob.Processor.Invoke(itemsToProcess);

                // remove selected items from the buffer for specific job id
                itemsToProcess.ForEach(x => ExecutingJobItems.FirstOrDefault(i => i.Key == jobItem.Key).Value.Remove(x));

                if (!jobItem.Value.Any())
                    FinishExecutingJob(executingJob);
            }
        }

        private void FinishExecutingJob(GeolocationBatchUpdateJob job)
        {
            ExecutingJobItems.Remove(job.Id);
            ExecutingJobs.Remove(job);
        }

        private void PendingJobsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                TryQueueNewJobs();
        }
    }
}
