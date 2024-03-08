myApp
    .factory('replacementReceiveCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchReplacementReceiveNumberr = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00235'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }
            return fac;
        }
    ]);