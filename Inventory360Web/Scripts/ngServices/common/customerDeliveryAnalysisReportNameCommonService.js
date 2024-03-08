myApp
    .factory('customerDeliveryAnalysisReportNameCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchcustomerDeliveryAnalysisReportName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/OS201'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);