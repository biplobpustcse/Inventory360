myApp
    .controller('convertionApproveController', ['$scope', '$ngConfirm', 'dataTableRows', 'applicationBasePath', 'userService', 'currencyCommonService', 'unApprovedConvertionService', 'convertionApproveService',
        function (
            $scope,
            $ngConfirm,
            dataTableRows,
            applicationBasePath,
            userService,
            currencyCommonService,
            unApprovedConvertionService,
            convertionApproveService
        ) {
            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.unApprovedConvertionJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetUnApprovedConvertionGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetUnApprovedConvertionGridListsData();
                }
            };

            $scope.filterConvertionNo = function () {
                if ($scope.searchConvertionNo !== undefined) {
                    $scope.pageIndex = 1;
                    GetUnApprovedConvertionGridListsData();
                }
            };

            $scope.loadCollectionNoByCurrency = function () {
                GetUnApprovedConvertionGridListsData();
            };

            $scope.approveCollection = function (id, collectionNo) {
                $ngConfirm({
                    title: 'Alert',
                    icon: 'glyphicon glyphicon-info-sign',
                    theme: 'supervan',
                    content: 'Are you sure you want to approve Convertion no. <strong>' + collectionNo + '</strong>',
                    animation: 'scale',
                    closeAnimation: 'scale',
                    buttons: {
                        Yes: {
                            btnClass: "btn-green",
                            action: function () {
                                convertionApproveService.ToApproveConvertion(
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
                GetUnApprovedConvertionGridListsData();
            };

            $scope.PrintReport = function (id) {
                var url = applicationBasePath + "/Inventory360Reports/PrintConvertion?id=" + id + "&user=" + $scope.UserName + "&currency=" + $scope.currencyId + "&token=" + userService.GetCurrentUser().access_token;
                var win = window.open(url, '_blank');
                win.focus();
            };

            // load currency
            var GetCurrencyForDropdown = function () {
                currencyCommonService.FetchCurrency().then(function (value) {
                    $scope.currencyDropDownJsonData = value.data;
                    $scope.currencyId = $scope.DefaultCurrency;
                    GetUnApprovedConvertionGridListsData();
                }, function (reason) {
                    console.log(reason);
                });
            }

            // load all Convertion lists
            var GetUnApprovedConvertionGridListsData = function () {
                var q = $scope.searchConvertionNo === undefined ? '' : $scope.searchConvertionNo;

                unApprovedConvertionService.FetchUnApprovedConvertionLists(
                    //q, $scope.currencyId, $scope.pageIndex, dataTableRows
                    q,$scope.pageIndex, dataTableRows
                ).then(function (value) {
                    $scope.unApprovedConvertionJsonLists = value.data;
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

            GetCurrencyForDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });