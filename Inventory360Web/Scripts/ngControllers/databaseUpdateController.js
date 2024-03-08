myApp
    .controller('databaseUpdateController', ['$scope', '$ngConfirm', 'databaseUpdateService',
        function (
            $scope,
            $ngConfirm,
            databaseUpdateService
        ) {
            $scope.clickUpdate = function () {
                databaseUpdateService.UpdateFullDatabase().then(function (value) {
                    if (Number(value.status) === 200 && value.data === true) {
                        // alert
                        $ngConfirm({
                            title: 'Success',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: 'Database updated successfully.',
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
        }
    ])
    .config(function ($locationProvider) {
        //default = 'false'
        $locationProvider.html5Mode(true);
    });