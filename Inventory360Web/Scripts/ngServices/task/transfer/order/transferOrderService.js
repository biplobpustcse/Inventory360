myApp
    .factory('transferOrderService', ['$http', 'serviceBasePath',
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
            fac.FetchTransferOrderSearch = function (transferTo, fromDate, toDate,fromStockType,toStockType) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0411'+
                    '?transferTo='+transferTo+
                        '&fromDate='+fromDate+
                        '&toDate='+ toDate+
                        '&fromStockType='+fromStockType+
                        '&toStockType='+ toStockType
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductWarehouseByLocation = function (productid, orderid) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0412' +
                        '?productid=' + productid +
                        '&orderid=' + orderid 
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }     
            fac.FetchProductSerialNos = function (productid, orderId) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0413' +
                        '?productid=' + productid +
                        '&orderId=' + orderId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductDetailInfo = function (productId, orderid) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0414' +
                        '?productid=' + productId +
                        '&orderId=' + orderid
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };
            fac.FetchProductWarehouseByLocationForSerialProduct = function (productid, orderId) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0415' +
                        '?productid=' + productid +
                        '&orderId=' + orderId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }     
            fac.FetchWarehouseBasedSerialNo = function (warehouseId, productid, orderid) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TS0416' +
                        '?productid=' + productid +
                        '&warehouseId=' + warehouseId+
                        '&orderId=' + orderid
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };
            return fac;
        }
    ]);