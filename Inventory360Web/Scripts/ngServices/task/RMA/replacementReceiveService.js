myApp
    .factory('replacementReceiveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.GetReplacementReceiveProductOption = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00221'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchProductNameByReplacementClaim = function (q, claimId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00237'
                        + '?query=' + q
                        + '&claimId=' + claimId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchProductBySerialFromReplacementClaim = function (q, productId, claimId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00238'
                        + '?serial=' + q
                        + '&productId=' + productId
                        + '&claimId=' + claimId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchReplacementClaimInfoByProduct = function (productId, productSerial, claimId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00239'
                        + '?productId=' + productId
                        + '&productSerial=' + productSerial
                        + '&claimId=' + claimId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchAllReplacementClaimNo = function (q, supplierId, dateFrom, dateTo) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0086'
                        + '?query=' + q
                        + '&supplierId=' + supplierId
                        + '&dateFrom=' + dateFrom
                        + '&dateTo=' + dateTo
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchAllReplacementClaimShortInfoById = function (claimId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0087'
                        + '?id=' + claimId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchAllReplacementClaimDetail_Problem = function (claimDetailId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00240'
                        + '?claimDetailId=' + claimDetailId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.SaveReplacementReceive = function (
                receiveDate, selectedEmployeeId, remarks, selectedCurrency, addedProductLists, replacementReceive_Charge, totalChargeAmount, discount
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00203',
                    traditional: true,
                    dataType: 'json',
                    data: JSON.stringify({
                        "ReceiveDate": receiveDate,
                        "RequestedBy": selectedEmployeeId,
                        "Remarks": remarks,
                        "SelectedCurrency": selectedCurrency,
                        "TotalChargeAmount": totalChargeAmount,
                        "TotalDiscount": discount,
                        "ReplacementReceiveDetail": addedProductLists,
                        "ReplacementReceive_Charge": replacementReceive_Charge
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            fac.FetchReplacementReceiveLists = function (q, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00217'
                        + '?query=' + q
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            };

            return fac;
        }
    ]);