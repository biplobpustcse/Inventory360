myApp
    .factory('replacementClaimService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

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
            };

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
            };

            fac.FetchAllProblemWithComplainReceived = function (ReceiveDetailId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00218'
                        + '?ReceiveDetailId=' + ReceiveDetailId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchReplacementClaimLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00215'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            fac.SaveReplacementClaim = function (
                selectedSupplierId, claimDate, selectedEmployeeId, remarks, SelectedCurrency, addedProductLists
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00202',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        "SupplierId": selectedSupplierId,
                        "ClaimDate": claimDate,
                        "RequestedBy": selectedEmployeeId,
                        "Remarks": remarks,
                        "SelectedCurrency": SelectedCurrency,
                        "replacementClaimDetail": addedProductLists
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            return fac;
        }
    ]);