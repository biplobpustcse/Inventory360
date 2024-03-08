myApp
    .factory('customerDeliveryService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchCustomerDeliveryLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00206'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };
            fac.SaveCustomerDelivery = function (
                currency, exchangeRate, selectedCustomerId, customerDeliveryDate, selectedEmployeeId,
                remarks, TotalChargeAmount, totalAmount, discount, customerDeliveryCharge,
                addedProductLists
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00201',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        "CustomerId": selectedCustomerId,
                        "DeliveryDate": customerDeliveryDate,
                        "RequestedBy": selectedEmployeeId,
                        "Remarks": remarks,
                        "SelectedCurrency": currency,   
                        "ExchangeRate": exchangeRate,
                        "TotalChargeAmount": TotalChargeAmount,     
                        "TotalAmount": totalAmount,
                        "Discount": discount,     
                        "CustomerDeliveryCharge": customerDeliveryCharge,
                        "CustomerDeliveryDetail": addedProductLists                        
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }
            fac.GetCusDelvProductOption = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00221'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetAdjustmentType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00224'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchAllProblemWithComplainReceived = function (ReceiveDetailId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00210'
                        + '?ReceiveDetailId=' + ReceiveDetailId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchAllChargeWithComplainReceived = function (ComplainReceiveId, currency) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00211'
                        + '?ComplainReceiveId=' + ComplainReceiveId
                        + '&currency=' + currency
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchSpareProductByParentRcvdIdAndProduct = function (ComplainReceiveId, ProductId, Serial) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00212'
                        + '?ComplainReceiveId=' + ComplainReceiveId
                        + '&ProductId=' + ProductId
                        + '&Serial=' + Serial
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);