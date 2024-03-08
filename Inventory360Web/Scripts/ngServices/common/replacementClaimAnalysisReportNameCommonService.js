myApp
    .factory('replacementClaimAnalysisReportNameCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchClaimAnalysisReportName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/OS202'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);