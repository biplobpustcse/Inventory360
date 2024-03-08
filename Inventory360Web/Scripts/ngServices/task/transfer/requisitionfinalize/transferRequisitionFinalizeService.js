myApp
    .factory('transferRequisitionFinalizeService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.SaveStockRequisitionFinalize = function (
                requisitionDate, requisitionBy, remarks, detailList, requisitionTo, stockType
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00400',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        "RequisitionDate": requisitionDate,
                        "RequisitionBy": requisitionBy,
                        "Remarks": remarks,
                        "FinalizeDetailLists": detailList,
                        "TransferToLocationId": requisitionTo,
                        "StockType": stockType
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }
            return fac;
        }
    ]);