myApp
    .factory('convertionCancelService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.TocancelConvertion = function (id, reason) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE203'
                        + '?id=' + id
                        + '&reason=' + reason
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);