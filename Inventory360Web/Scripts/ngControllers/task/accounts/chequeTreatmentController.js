myApp
    .controller('chequeTreatmentController', ['$scope', '$filter', '$ngConfirm', 'dataTableRows', 'accountService', 'bankCommonService', 'supplierCommonService', 'customerCommonService', 'allCollectionService', 'applicationBasePath', 'userService', 'currencyCommonService', 'customerCommonService', 'securityUserCommonService', 'collectionAgainstCommonService', 'paymentModeCommonService', 'bankCommonService', 'chequeTreatmentService',
        function (
            $scope, $filter, $ngConfirm, dataTableRows, accountService, bankCommonService, supplierCommonService, customerCommonService, allCollectionService, applicationBasePath, userService, currencyCommonService,
            customerCommonService, securityUserCommonService, collectionAgainstCommonService, paymentModeCommonService,
            bankCommonService, chequeTreatmentService
        ) {
            $scope.selectedCurrency = $scope.DefaultCurrency;
            $scope.dateFrom = $filter('date')(new Date(), "M/d/yyyy");
            $scope.dateTo = $filter('date')(new Date(), "M/d/yyyy");
            $scope.treatmentDate = $filter('date')(new Date(), "M/d/yyyy");
            //--------------------------
            $scope.selectedLocationId = 0;
            $scope.allChequeSelectedData = [];

            $scope.pageIndex = 1;
            $scope.showRecordsInfo = '';
            $scope.showGrid = false;
            $scope.allChequeJsonLists = [];

            $scope.previous = function (e) {
                e.preventDefault();
                if ($scope.pageIndex > 1) {
                    $scope.pageIndex--;
                    GetAllChequeGridListsData();
                }
            };

            $scope.next = function (e) {
                e.preventDefault();
                if ($scope.pageIndex < lastPageNo) {
                    $scope.pageIndex++;
                    GetAllChequeGridListsData();
                }
            };

            $scope.filterChequeNo = function () {
                $scope.pageIndex = 1;
                if ($scope.dateFrom && $scope.dateTo) {
                    if (Date.parse($scope.dateTo) < Date.parse($scope.dateFrom)) {
                        alert("From Date can't be greater than Date To!!!");
                        return;
                    }
                }
                GetAllChequeGridListsData();
            };

            $scope.checkedAll = function () {
                $.each($scope.allChequeJsonLists.Data, function () {
                    this.isSelected = $scope.checkAll;
                });
            };

            $scope.clickToSave = function () {
                // chequeTreatement 
                $scope.allChequeSelectedData = [];
                if ($scope.chequeStatusCode == "N" && !$scope.chequeSendingBankId) {
                    alert("Please Select Cheque Sending Bank!!!");
                    return;
                }
                $.each($scope.allChequeJsonLists.Data, function () {
                    if (this.isSelected) {
                        $scope.allChequeSelectedData.push({
                            "ChequeInfoId": this.ChequeInfoId,
                            "PreviousStatus": this.Status,
                            "Status": $scope.treatmentStatusCode,
                            "StatusDate": $scope.treatmentDate,
                            "PreviousTreatmentBankId": this.SendBankId,
                            "TreatmentBankId": $scope.chequeSendingBankId,
                            "VoucherId": this.voucherId
                        });
                    }
                });
                if ($scope.allChequeSelectedData.length < 1) {
                    alert("Please Select at least one Cheque!!!");
                    return;
                }
                //$scope.isSaved = true;
                chequeTreatmentService.SavechequeTreatment($scope.allChequeSelectedData
                ).then(function (value) {
                    if (Number(value.status) === 200 && value.data.IsSuccess === true) {
                        $scope.collectionNo = value.data.Message;
                        // alert
                        $ngConfirm({
                            title: 'Success',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: 'Save successful.',
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });
                        GetAllChequeGridListsData();
                    } else {
                        // alert
                        $ngConfirm({
                            title: 'Failed',
                            icon: 'glyphicon glyphicon-info-sign',
                            theme: 'supervan',
                            content: value.data.Message,
                            animation: 'scale',
                            buttons: {
                                Ok: {
                                    btnClass: "btn-blue"
                                }
                            },
                        });

                        $scope.isSaved = false;
                    }
                });
            };

            // load ChequeType
            var GetChequeTypeForDropdown = function () {
                chequeTreatmentService.GetChequeType().then(function (value) {
                    $scope.chequeTypeDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load ChequeStatus
            var GetChequeStatusForDropdown = function () {
                chequeTreatmentService.GetChequeStatus().then(function (value) {
                    $scope.srcChequeStatusDropDownJsonData = value.data;
                }, function (reason) {
                    console.log(reason);
                });
            }
            //GetChequeStatusByGroup
            var GetChequeStatusByGroup = function () {
                chequeTreatmentService.GetChequeStatusByGroup($scope.chequeStatusCode).then(function (value) {
                    $scope.chequeStatusDropDownJsonData = value.data;
                    if ($scope.chequeStatusDropDownJsonData.length == 1) {
                        $scope.treatmentStatusCode = $scope.chequeStatusDropDownJsonData[0].Value;
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load all company
            var getAllCompanyForDropdown = function () {
                accountService.fetchAllCompany().then(function (value) {
                    $scope.allCompanyJsonData = value.data;
                    if (value.data.length === 1) {
                        $scope.companyId = value.data[0].CompanyId.toString();
                        $scope.getLocationByCompanyForDropdown();
                    }
                }, function (reason) {
                    console.log(reason);
                });
            }
            // load all parent location by selected company
            $scope.getLocationByCompanyForDropdown = function () {
                accountService.fetchLocationByCompany($scope.companyId).then(function (value) {
                    if (value.data.length === 2) {
                        $scope.selectedLocationId = value.data[1].Value.toString();
                    }
                    else {
                        $.each(value.data, function () {
                            if (this.IsSelected) {
                                $scope.selectedLocationId = this.Value;
                            }
                        });
                    }
                }, function (reason) {
                    console.log(reason);
                });
            };
            //load all bank           
            var getAllBankForDropdown = function () {
                bankCommonService.FetchBank("").then(function (value) {
                    $scope.tempAllBankJsonData = value.data;
                    $scope.ownBankJsonData = angular.copy($scope.tempAllBankJsonData);
                }, function (reason) {
                    console.log(reason);
                });
            }

            //load own bank           
            var getOwnBankByCompanyIdForDropdown = function () {
                bankCommonService.FetchOwnBank("").then(function (value) {
                    $scope.ownBankJsonData = value.data;
                    $scope.tempOwnBankJsonData = angular.copy($scope.ownBankJsonData);
                }, function (reason) {
                    console.log(reason);
                });
            }
            //change Status
            $scope.changeStatus = function () {
                GetChequeStatusByGroup();
            }
            //getCustomerOrSupplierName
            $scope.getCustomerOrSupplierName = function () {
                var q = $scope.customerOrSupplierName === undefined ? '' : $scope.customerOrSupplierName;

                if (q.length >= 3) {
                    if ($scope.chequeTypValue === "issuedCheque") {
                        supplierCommonService.FetchAllSupplier(q).then(function (value) {
                            $scope.customerOrSupplierNameJsonData = value.data;
                        });
                    }
                    else {
                        customerCommonService.FetchAllCustomer(q).then(function (value) {
                            $scope.customerOrSupplierNameJsonData = value.data;
                        });
                    }
                } else {
                    $scope.selectedCustomerOrSupplierId = 0;
                    $scope.customerOrSupplierNameJsonData = null;
                }
            };
            $scope.fillCustomerTextbox = function (obj) {
                $scope.customerOrSupplierName = '';
                $scope.selectedCustomerOrSupplierId = Number(obj.Value);
                $scope.customerOrSupplierName = obj.Item;
                $scope.customerOrSupplierNameJsonData = null;
                GetAllChequeGridListsData();
            };
            // load all collection lists
            var GetAllChequeGridListsData = function () {
                var searchQuery = $scope.searchChequeNo === undefined ? '' : $scope.searchChequeNo;
                if ($scope.chequeTypValue === "issuedCheque") {
                    $scope.customerOrSupplier = "Supplier";
                    $scope.collectionOrPayment = "Payment";
                    $scope.CustomerOrSupplier = "Supplier";
                    $scope.allBankJsonData = $scope.tempOwnBankJsonData;
                    $scope.ownBankJsonData = $scope.tempOwnBankJsonData;
                }
                else {
                    $scope.customerOrSupplier = "Customer";
                    $scope.collectionOrPayment = "Collection";
                    $scope.CustomerOrSupplier = "Customer";
                    $scope.allBankJsonData = $scope.tempOwnBankJsonData;
                    $scope.ownBankJsonData = $scope.tempAllBankJsonData;
                }

                chequeTreatmentService.FetchAllChequeInfoLists(searchQuery, ($scope.chequeTypValue === undefined ? '' : $scope.chequeTypValue), $scope.dateFrom, $scope.dateTo, ($scope.chequeStatusCode === undefined ? '' : $scope.chequeStatusCode), $scope.selectedLocationId, ($scope.ownBankId === undefined ? 0 : $scope.ownBankId), ($scope.selectedCustomerOrSupplierId == undefined ? 0 : $scope.selectedCustomerOrSupplierId), $scope.currencyId, $scope.pageIndex, dataTableRows).then(function (value) {
                    $scope.allChequeJsonLists = value.data;
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
            getAllCompanyForDropdown();
            GetChequeTypeForDropdown();
            GetChequeStatusForDropdown();
            getAllBankForDropdown();
            GetAllChequeGridListsData();
            getOwnBankByCompanyIdForDropdown();
        }
    ])
    .config(function ($locationProvider) {
        $locationProvider.html5Mode(true);
    });