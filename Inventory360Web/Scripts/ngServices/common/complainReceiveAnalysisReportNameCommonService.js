myApp
    .factory('complainReceiveAnalysisReportNameCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSalesReportName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/OS200'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);