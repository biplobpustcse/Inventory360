myApp
    .factory('convertionApproveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToApproveConvertion = function (id) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE204'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);