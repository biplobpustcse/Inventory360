myApp
    .controller('transferReqFinalizeCancelController', ['$scope', '$ngConfirm', 'dataTableRows', 'applicationBasePath', 'userService',
        'unApprovedTransferRequisitionFinalizeService', 'transferRequisitionFinalizeWithDetailService', 'transferRequisitionFinalizeCancelService',
        function (
            $scope, $ngConfirm, dataTableRows, applicationBasePath, userService,
            unApprovedTransferRequisitionFinalizeService, transferRequisitionFinalizeWithDetailService, transferRequisitionFinalizeCancelService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.unApprovedRequisitionFinalizeJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetUnApprovedTranReqFinalizeListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetUnApprovedTranReqFinalizeListsData();
                }
            };

            $scope.filterFinalizeNo = function () {
                if ($scope.searchFinalizeNo !== undefined) {
                    $scope.pageIndex = 1;
                    GetUnApprovedTranReqFinalizeListsData();
                }
            };

            $scope.showRequisitionFinalizeDetail = function (id) {
                $scope.requisitionFinalizeDetailJsonData = [];
                transferRequisitionFinalizeWithDetailService.FetchSelectedFinalizeDetail(
                    id
                ).then(function (value) {
                    $scope.requisitionFinalizeDetailJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            };

            $scope.cancelRequisitionFinalize = function (id, requisitionNo) {
                $ngConfirm({
                    title: 'Alert',
                    content: '<div class="form-group">Are you sure you want to cancel requisition finalize no. <strong>' + requisitionNo + '</strong></div>'
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
                                transferRequisitionFinalizeCancelService.ToCancelRequisitionFinalize(
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
                GetUnApprovedTranReqFinalizeListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndTransferRequisition?id=" + id + "&user=" + $scope.UserName + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load all Un Aproved Requisition Finalize lists
            var GetUnApprovedTranReqFinalizeListsData = function () {
                var q = $scope.searchFinalizeNo === undefined ? '' : $scope.searchFinalizeNo;

                unApprovedTransferRequisitionFinalizeService.FetchUnApprovedStockRequisitionLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.unApprovedRequisitionFinalizeJsonLists = value.data;
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

            GetUnApprovedTranReqFinalizeListsData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });