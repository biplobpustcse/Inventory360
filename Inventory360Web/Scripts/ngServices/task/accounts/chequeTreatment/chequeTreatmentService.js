myApp
    .factory('chequeTreatmentService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};
            fac.GetChequeType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00201'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetChequeStatus = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00202'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetChequeStatusByGroup = function (GRP) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00205'
                        + '?GRP=' + GRP
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            //fac.FetchAllChequeInfoLists = function (dateFrom, dateTo, chequeStatusCode, locationId, ownBankId,searchQuery, currency, pageIndex, pageSize) {
            fac.FetchAllChequeInfoLists = function (searchQuery, chequeTypValue, dateFrom, dateTo, chequeStatusCode, selectedLocationId, ownBankId, selectedCustomerOrSupplierId, currency, pageIndex, pageSize) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00203'

                        + '?searchQuery=' + searchQuery
                        + '&chequeTypValue=' + chequeTypValue
                        + '&dateFrom=' + dateFrom
                        + '&dateTo=' + dateTo
                        + '&chequeStatusCode=' + chequeStatusCode
                        + '&selectedLocationId=' + selectedLocationId
                        + '&ownBankId=' + ownBankId
                        + '&CustomerOrSupplierId=' + selectedCustomerOrSupplierId
                        + '&currency=' + currency
                        + '&pageIndex=' + pageIndex
                        + '&pageSize=' + pageSize
                    //+ '#' + Math.random().toString(36)
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.SavechequeTreatment = function (allChequeSelectedData)
                {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI00204',
                    dataType: 'json',
                    data: JSON.stringify({
                        "CommonTaskChequeTreatmentLists": allChequeSelectedData
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }  
            fac.GetChequePerformanceType = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00210'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);