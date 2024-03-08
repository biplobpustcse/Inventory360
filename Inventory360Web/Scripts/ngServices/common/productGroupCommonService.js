myApp
    .factory('productGroupCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductGroup = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0018'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);