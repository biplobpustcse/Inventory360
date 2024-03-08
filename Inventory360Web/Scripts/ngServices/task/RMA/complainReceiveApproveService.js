myApp
    .factory('complainReceiveApproveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToapproveComplainReceive = function (id) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE201'
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