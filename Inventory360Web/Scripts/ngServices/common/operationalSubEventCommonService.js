myApp
    .factory('operationalSubEventCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchOperationalSubEvent = function (q, event) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0040'
                        + '?query=' + q
                        + '&eventName=' + event
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            return fac;
        }
    ]);