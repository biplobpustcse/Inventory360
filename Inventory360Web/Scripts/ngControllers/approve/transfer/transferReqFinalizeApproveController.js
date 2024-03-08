myApp
    .controller('transferReqFinalizeApproveController', ['$scope', '$ngConfirm', 'dataTableRows', 'applicationBasePath', 'userService',
        'unApprovedTransferRequisitionFinalizeService', 'transferRequisitionFinalizeWithDetailService', 'transferRequisitionFinalizeApproveService',
        function (
            $scope, $ngConfirm, dataTableRows, applicationBasePath, userService,
            unApprovedTransferRequisitionFinalizeService, transferRequisitionFinalizeWithDetailService, transferRequisitionFinalizeApproveService
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

            $scope.approveRequisitionFinalize = function (id, requisitionNo) {
                $ngConfirm({
                    title: 'Alert',
                    icon: 'glyphicon glyphicon-info-sign',
                    theme: 'supervan',
                    content: 'Are you sure you want to approve Transfer Requisition Finalize No. <strong>' + requisitionNo + '</strong>',
                    animation: 'scale',
                    closeAnimation: 'scale',
                    buttons: {
                        Yes: {
                            btnClass: "btn-green",
                            action: function () {
                                transferRequisitionFinalizeApproveService.ToApproveRequisitionFinalize(
                                    id
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