myApp
    .factory('collectionApproveService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.ToApproveCollection = function (id) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TE014'
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