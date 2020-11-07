using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Assessment.IpGeolocation.Common.Interfaces;

namespace Assessment.IpGeolocation.Common.Models
{
    public abstract class BackgroundJob : IBackgroundJob
    {
        public Guid Id { get; }
        public object Request { get; }

        protected BackgroundJob(object request)
        {
            Id = Guid.NewGuid();
            Request = request;
        }
    }

    public class BackgroundJobStatus
    {
        [JsonIgnore]
        public int TotalItemCount { get; set; }
        [JsonIgnore]
        public int RemainingItemCount { get; set; }

        public bool IsCompleted => RemainingItemCount == 0;
        public string Progress => $"{TotalItemCount - RemainingItemCount}/{TotalItemCount}";
    }
}
