﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CC
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Antigen> Antigens { get; set; }
        public virtual DbSet<ArrayAntigen> ArrayAntigens { get; set; }
        public virtual DbSet<Array> Arrays { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<CalibControl> CalibControls { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<ArrayAntigenRelations_Result> GetArrayAntigenRelations()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ArrayAntigenRelations_Result>("GetArrayAntigenRelations");
        }
    
        public virtual ObjectResult<AntigensAssingedToArray> GetAntigensAssingedToArray(string arrayId)
        {
            var arrayIdParameter = arrayId != null ?
                new ObjectParameter("arrayId", arrayId) :
                new ObjectParameter("arrayId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AntigensAssingedToArray>("GetAntigensAssingedToArray", arrayIdParameter);
        }
    
        public virtual ObjectResult<AntigensNotAssingedToArray_Result> GetAntigensNotAssingedToArray()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AntigensNotAssingedToArray_Result>("GetAntigensNotAssingedToArray");
        }
    
        public virtual ObjectResult<ActiveCCs> GetExistingCCs(string arrayId, string antigenId, string type)
        {
            var arrayIdParameter = arrayId != null ?
                new ObjectParameter("arrayId", arrayId) :
                new ObjectParameter("arrayId", typeof(string));
    
            var antigenIdParameter = antigenId != null ?
                new ObjectParameter("antigenId", antigenId) :
                new ObjectParameter("antigenId", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ActiveCCs>("GetExistingCCs", arrayIdParameter, antigenIdParameter, typeParameter);
        }
    
        public virtual ObjectResult<Array> GetArrayByLIMNumber(string lIMArrayNumber)
        {
            var lIMArrayNumberParameter = lIMArrayNumber != null ?
                new ObjectParameter("LIMArrayNumber", lIMArrayNumber) :
                new ObjectParameter("LIMArrayNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Array>("GetArrayByLIMNumber", lIMArrayNumberParameter);
        }
    
        public virtual ObjectResult<Array> GetArrayByLIMNumber(string lIMArrayNumber, MergeOption mergeOption)
        {
            var lIMArrayNumberParameter = lIMArrayNumber != null ?
                new ObjectParameter("LIMArrayNumber", lIMArrayNumber) :
                new ObjectParameter("LIMArrayNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Array>("GetArrayByLIMNumber", mergeOption, lIMArrayNumberParameter);
        }
    
        public virtual ObjectResult<Batch> GetBatchRecords(string batchName, Nullable<System.DateTime> runDate, Nullable<int> blockNumber, string antigenGroup)
        {
            var batchNameParameter = batchName != null ?
                new ObjectParameter("batchName", batchName) :
                new ObjectParameter("batchName", typeof(string));
    
            var runDateParameter = runDate.HasValue ?
                new ObjectParameter("runDate", runDate) :
                new ObjectParameter("runDate", typeof(System.DateTime));
    
            var blockNumberParameter = blockNumber.HasValue ?
                new ObjectParameter("BlockNumber", blockNumber) :
                new ObjectParameter("BlockNumber", typeof(int));
    
            var antigenGroupParameter = antigenGroup != null ?
                new ObjectParameter("antigenGroup", antigenGroup) :
                new ObjectParameter("antigenGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Batch>("GetBatchRecords", batchNameParameter, runDateParameter, blockNumberParameter, antigenGroupParameter);
        }
    
        public virtual ObjectResult<Batch> GetBatchRecords(string batchName, Nullable<System.DateTime> runDate, Nullable<int> blockNumber, string antigenGroup, MergeOption mergeOption)
        {
            var batchNameParameter = batchName != null ?
                new ObjectParameter("batchName", batchName) :
                new ObjectParameter("batchName", typeof(string));
    
            var runDateParameter = runDate.HasValue ?
                new ObjectParameter("runDate", runDate) :
                new ObjectParameter("runDate", typeof(System.DateTime));
    
            var blockNumberParameter = blockNumber.HasValue ?
                new ObjectParameter("BlockNumber", blockNumber) :
                new ObjectParameter("BlockNumber", typeof(int));
    
            var antigenGroupParameter = antigenGroup != null ?
                new ObjectParameter("antigenGroup", antigenGroup) :
                new ObjectParameter("antigenGroup", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Batch>("GetBatchRecords", mergeOption, batchNameParameter, runDateParameter, blockNumberParameter, antigenGroupParameter);
        }
    
        public virtual ObjectResult<GetArrayAntigens_Result> GetArrayAntigens(string arrayId)
        {
            var arrayIdParameter = arrayId != null ?
                new ObjectParameter("arrayId", arrayId) :
                new ObjectParameter("arrayId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetArrayAntigens_Result>("GetArrayAntigens", arrayIdParameter);
        }
    }
}
