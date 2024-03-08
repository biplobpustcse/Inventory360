myApp
    .controller('complainReceiveApproveController', ['$scope', '$ngConfirm', 'dataTableRows', 'applicationBasePath', 'userService', 'unApprovedComplainReceiveService', 'complainReceiveApproveService',
        function (
            $scope,
            $ngConfirm,
            dataTableRows,
            applicationBasePath,
            userService,           
            unApprovedComplainReceiveService,
            complainReceiveApproveService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.unApprovedComplainReceiveJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetUnApprovedComplainReceiveGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetUnApprovedComplainReceiveGridListsData();
                }
            };

            $scope.filterCollectionNo = function () {
                if ($scope.searchComplainReceiveNo !== undefined) {
                    $scope.pageIndex = 1;
                    GetUnApprovedComplainReceiveGridListsData();
                }
            };

            $scope.approveComplainReceive = function (id, ReceiveNo) {
                $ngConfirm({
                    title: 'Alert',
                    icon: 'glyphicon glyphicon-info-sign',
                    theme: 'supervan',
                    content: 'Are you sure you want to approve Complain Receive no. <strong>' + ReceiveNo + '</strong>',
                    animation: 'scale',
                    closeAnimation: 'scale',
                    buttons: {
                        Yes: {
                            btnClass: "btn-green",
                            action: function () {
                                complainReceiveApproveService.ToapproveComplainReceive(
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
                GetUnApprovedComplainReceiveGridListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintIndComplainReceive?id=" + id + "&user=" + $scope.UserName + "&currency=" + $scope.DefaultCurrency + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load all complain receive lists
            var GetUnApprovedComplainReceiveGridListsData = function () {
                var q = $scope.searchComplainReceiveNo === undefined ? '' : $scope.searchComplainReceiveNo;

                unApprovedComplainReceiveService.FetchUnApprovedComplainReceiveLists(
                    q, $scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.unApprovedComplainReceiveJsonLists = value.data;
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
            $scope.loadData();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });