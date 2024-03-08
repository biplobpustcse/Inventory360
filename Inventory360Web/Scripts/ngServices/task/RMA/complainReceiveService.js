myApp
    .factory('complainReceiveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchComplainReceiveLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00205'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };
            fac.SaveComplainReceive = function (
                selectedCustomerId, complainReceiveDate, selectedEmployeeId, remarks, SelectedCurrency,
                AgainstPreviousSales, TotalChargeAmount, complainReceive_ChargeList, addedProductLists, exchangeRate
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00200',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({        
                        "CustomerId": selectedCustomerId,
                        "ReceiveDate": complainReceiveDate,
                        "RequestedBy": selectedEmployeeId,
                        "Remarks": remarks,
                        "SelectedCurrency": SelectedCurrency,
                        "ExchangeRate": exchangeRate,
                        "AgainstPreviousSales": AgainstPreviousSales,
                        "TotalChargeAmount": TotalChargeAmount,
                        "complainReceive_Charge": complainReceive_ChargeList,
                        "complainReceiveDetail": addedProductLists
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }
            fac.FetchAllComplainReceiveNo = function (q, CustomerId, dateFrom, dateTo) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00207'
                        + '?query=' + q
                        + '&CustomerId=' + CustomerId
                        + '&dateFrom=' + dateFrom
                        + '&dateTo=' + dateTo
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };
            fac.FetchProductNameByComplainReceive = function (q, complainReceiveId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00208'
                        + '?query=' + q
                        + '&ComplainReceiveId=' + complainReceiveId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchProductBySerialFromComplainReceive = function (q, ProductId, complainReceiveId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00209'
                        + '?serial=' + q
                        + '&ProductId=' + ProductId
                        + '&complainReceiveId=' + complainReceiveId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchComplainReceiveInfoByProduct = function (ProductId, productSerial, ComplainReceiveId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00222'
                        + '?ProductId=' + ProductId
                        + '&ProductSerial=' + productSerial
                        + '&ComplainReceiveId=' + ComplainReceiveId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetSpareType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00225'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchAllComplainReceiveShortInfoById = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00226'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };
            fac.FetchRMAProductBySerialFromComplainReceive = function (q, ProductId, complainReceiveId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00214'
                        + '?serial=' + q
                        + '&ProductId=' + ProductId
                        + '&complainReceiveId=' + complainReceiveId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.FetchRMAProductNameByComplainReceive = function (q, complainReceiveId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00216'
                        + '?query=' + q
                        + '&ComplainReceiveId=' + complainReceiveId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);