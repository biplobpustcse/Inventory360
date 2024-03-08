myApp
    .factory('collectionCancelService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToCancelCollection = function (id, reason) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE013'
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