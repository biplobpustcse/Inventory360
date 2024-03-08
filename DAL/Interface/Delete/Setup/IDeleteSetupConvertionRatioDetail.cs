using System;
using System.Collections.Generic;

namespace DAL.Interface.Delete.Setup
{
    public interface IDeleteSetupConvertionRatioDetail
    {
        bool DeleteConvertionRatioDetail(List<Guid> convertionRatioDetailIds);
    }
}