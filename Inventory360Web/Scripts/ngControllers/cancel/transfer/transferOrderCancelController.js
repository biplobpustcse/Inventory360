myApp
    .controller('transferOrderCancelController', ['$scope', '$ngConfirm', 'dataTableRows', 'applicationBasePath', 'userService',
        'unApprovedTransferOrderService', 'transferOrderWithDetailService', 'transferOrderCancelService',
        function (
            $scope, $ngConfirm, dataTableRows, applicationBasePath, userService,
            unApprovedTransferOrderService, transferOrderWithDetailService, transferOrderCancelService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.unApprovedTransferOrderJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetUnApprovedTransferOrderListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetUnApprovedTransferOrderListsData();
                }
            };

            $scope.filterTransferOrderNo = function () {
                if ($scope.searchTransferOrder !== undefined) {
                    $scope.pageIndex = 1;
                    GetUnApprovedTransferOrderListsData();
                }
            };

            $scope.showTransferOrderDetail = function (id) {
                $scope.transferOrderDetailJsonData = [];
                transferOrderWithDetailService.FetchSelectedTransferOrderDetail(
                    id
                ).then(function (value) {
                    $scope.transferOrderDetailJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.cancelTransferOrder = function (id, orderNo) {
                $ngConfirm({
                    title: 'Alert',
                    content: '<div class="form-group">Are you sure you want to cancel transfer order no. <strong>' + orderNo + '</strong></div>'
                        + '<div class="form-group">'
                        + '<textarea ng-change="textChange()" cols="20" rows="2" ng-model="reason" maxlength="200" placeholder="Give reason why you want to cancel..." class="form-control"></textarea>'
                        + '</div>',
                    icon: 'glyphicon glyphicon-info-sign',
                    theme: 'supervan',
                    animation: 'scale',
                    closeAnimation: 'scale',
                    buttons: {
                        Yes: {
                            disabled: true,
                            btnClass: 'btn-green',
                            action: function (scope) {
                                transferOrderCancelService.ToCancelTransferOrder(
                                    id, scope.reason
                                ).then(function (value) {
                                    var title = '';
                                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                                        title = 'Success';
                                    } else {
                                        title = 'Failed';
                                    }

                                    // alert
                                    $ngConfirm({
                                        title: title,
                                        icon: 'glyphicon glyphicon-info-sign',
                                        theme: 'supervan',
                                        content: value.data.Message,
                                        animation: 'scale',
                                        buttons: {
                                            Ok: {
                                                btnClass: "btn-blue",
                                                action: function () {
                                                    $scope.loadData();
                                                }
                                            }
                                        },
                                    });
                                }, function (reason) {
                                    console.log(reason);
                                });
                            }
                        },
                        No: function () {
                        }
                    },
                    onScopeReady: function (scope) {
                        var self = this;
                        scope.textChange = function () {
                            if (scope.reason)
                                self.buttons.Yes.setDisabled(false);
                            else
                                self.buttons.Yes.setDisabled(true);
                        }
                    }
                });
            };

            $scope.loadData = function () {
                $scope.pageIndex = 1;
                GetUnApprovedTransferOrderListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferOrder?id=" + id + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load all Un Aproved Transfer Order lists
            var GetUnApprovedTransferOrderListsData = function () {
                var q = $scope.searchTransferOrder === undefined ? '' : $scope.searchTransferOrder;

                unApprovedTransferOrderService.FetchUnApprovedTransferOrderLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.unApprovedTransferOrderJsonLists = value.data;
                    lastPageNo = value.data.LastPageNo;
                    if (value.data.TotalNumberOfRecords > 0) {
                        $scope.showGrid = true;
                        $scope.showRecordsInfo = 'Record(s) showing ' + value.data.Start + " to " + value.data.End + " of " + value.data.TotalNumberOfRecords;
                    } else {
                        $scope.showGrid = false;
                        $scope.showRecordsInfo = "No record(s) found...";
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }

            GetUnApprovedTransferOrderListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });



