using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupMeasurement : IUpdateSetupMeasurement
    {
        private Inventory360Entities _db;
        private Setup_Measurement _findEntity;

        public DUpdateSetupMeasurement(CommonSetupMeasurement entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Measurement.Find(entity.MeasurementId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.LengthValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Length.ToString()).Select(s => s.Value).FirstOrDefault();
            _findEntity.WidthValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Width.ToString()).Select(s => s.Value).FirstOrDefault();
            _findEntity.HeightValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Height.ToString()).Select(s => s.Value).FirstOrDefault();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateMeasurement()
        {
            try
            {
                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}