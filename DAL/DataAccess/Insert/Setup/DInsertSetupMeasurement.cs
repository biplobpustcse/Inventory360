using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupMeasurement : IInsertSetupMeasurement
    {
        private Inventory360Entities _db;
        private Setup_Measurement _entity;

        public DInsertSetupMeasurement(CommonSetupMeasurement entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Measurement
            {
                Code = entity.Code,
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
                LengthValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Length.ToString()).Select(s=>s.Value).FirstOrDefault(),
                WidthValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Width.ToString()).Select(s => s.Value).FirstOrDefault(),
                HeightValue = entity.MeasurementNamesList == null ? 0 : entity.MeasurementNamesList.Where(x => x.Name == CommonEnum.MeasurementName.Height.ToString()).Select(s => s.Value).FirstOrDefault()
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertMeasurement()
        {
            try
            {
                _db.Setup_Measurement.Add(_entity);
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