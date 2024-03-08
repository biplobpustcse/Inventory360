myApp
    .factory('complainReceiveCancelService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.TocancelComplainReceive = function (id, reason) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE202'
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