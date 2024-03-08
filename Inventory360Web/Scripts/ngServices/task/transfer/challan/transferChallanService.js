myApp
    .factory('transferChallanService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.SaveTransferOrder = function (
                requisitionDate, selectedEmployeeId, remarks, addedProductLists,
                requisitionTo, requisitionFrom, stockTypeTo, stockTypeFrom
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00408',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        "OrderDate": requisitionDate,
                        "OrderBy": selectedEmployeeId,
                        "Remarks": remarks,
                        "TransferOrderDetailList": addedProductLists,
                        "TransferToId": requisitionTo,
                        "TransferFromId": requisitionFrom,
                        "TransferToStockType": stockTypeTo,
                        "TransferFromStockType": stockTypeFrom
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