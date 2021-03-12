using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceRequests.Models
{
    public class ServiceModel
    {
        [Key]
        public Guid Id { get; set; } //: guid
        [Required]
        public string BuildingCode { get; set; }    //: string
        [Required]
        public string Description { get; set; }    //: string
        [Required]
        public CurrentStatus? CurrentStatus { get; set; }    //: enum
        [Required]
        public string CreatedBy { get; set; }    //: string
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }      //: dateTime
        [Required]
        public string LastModifiedBy { get; set; }    //: string
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow ; //: dateTime 

        public ServiceModel Copy(ServiceModel _serviceModel)
        {
            this.BuildingCode = _serviceModel.BuildingCode;
            this.Description = _serviceModel.Description;
            this.CurrentStatus = _serviceModel.CurrentStatus;
            this.CreatedBy = _serviceModel.CreatedBy;
            this.CreatedDate = _serviceModel.CreatedDate;
            this.LastModifiedBy = _serviceModel.LastModifiedBy;
            this.LastModifiedDate = _serviceModel.LastModifiedDate = DateTime.UtcNow;
            return this;
        }
    }
    public enum CurrentStatus { NotApplicable, Created, InProgress, Complete, Canceled }
}
