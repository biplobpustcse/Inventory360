myApp
    .controller('changePasswordController', ['$scope', '$ngConfirm', 'applicationBasePath', 'securityUserChangePasswordService',
        function (
            $scope,
            $ngConfirm,
            applicationBasePath,
            securityUserChangePasswordService
        ) {
            $scope.clickUpdate = function () {
                securityUserChangePasswordService.ChangePassword(
                    $scope.currentPassword, $scope.newPassword, $scope.confirmPassword
                ).then(function (value) {
                    // alert
                    $ngConfirm({
                        title: value.data.IsSuccess ? 'Success' : 'Failed',
                        icon: 'glyphicon glyphicon-info-sign',
                        theme: 'supervan',
                        content: value.data.Message,
                        animation: 'scale',
                        buttons: {
                            Ok: {
                                btnClass: "btn-blue",
                                action: function () {
                                    if (value.data.IsSuccess) {
                                        window.location.href = applicationBasePath + "/Home";
                                    }
                                }
                            }
                        },
                    });
                });
            };

            $scope.clickClose = function () {
                window.location.href = applicationBasePath + "/Home";
            };
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });