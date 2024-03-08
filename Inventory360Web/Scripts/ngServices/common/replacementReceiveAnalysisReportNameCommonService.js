myApp
    .factory('replacementReceiveAnalysisReportNameCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchReplacementReceiveAnalysisReportName = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/OS203'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);