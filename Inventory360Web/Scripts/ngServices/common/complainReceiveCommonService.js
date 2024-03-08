myApp
    .factory('complainReceiveCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchComplainReceiveNo = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00234'
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