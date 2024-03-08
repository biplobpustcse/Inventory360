myApp
    .factory('chequeTreatmentReportCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.GetReportName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00206'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetPositionOptionName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00207'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetChequeOrTreatementBankOption = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00208'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetChequeCollectionOrPaymentDateOptionByGroup = function (GRP) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00209' + '?GRP=' + GRP
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            fac.GetChequeNo = function (query) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS00204' + '?query=' + query
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            
            

            return fac;
        }
    ]);