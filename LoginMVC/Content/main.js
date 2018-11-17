let applicationData = null;
let currentApplicationId = null;
let userDetails = null;

$(document).ready(function () {
    let uiEvents = new UIEvents();

    $("#addApplicationDOM").click(() => {
        const applicationDetails = {
            "Name": $("#add-app-name").val(),
            "Description": $("#add-app-description").val(),
            "Version": $("#add-app-version").val(),
            "PlannedRelease": $("#add-app-plannedRelease").val(),
            "ActualRelease": $("#add-app-actualRelease").val(),
            "Environment": $("#add-app-environment").val(),
            "Client": $("#add-app-client").val(),
            "Server": $("#add-app-server").val(),
            "Technology": $("#add-app-technology").val(),
            "CreatedBy": "1",
            "Type": $("#add-app-type").val()
        };

        if (applicationDetails.Name && applicationDetails.Description && applicationDetails.Version && applicationDetails.PlannedRelease
            && applicationDetails.ActualRelease && applicationDetails.Environment && applicationDetails.Client && applicationDetails.Server
            && applicationDetails.Technology && applicationDetails.Type) {
            $.ajax({
                type: 'POST',
                url: 'AddApplication',
                data: JSON.stringify(applicationDetails),
                success: function (data) {
                    ClearApplication();
                    alert(`Application added succesfully`);
                },
                error: function (err) {
                    alert("Error in adding application");
                },
                contentType: "application/json",
                dataType: 'json'
            });
        }
        else {
            alert("Enter all details to add application.!!!");
        }
    });

    /* View application click event - from DOM */
    $("#viewApplicationTab").click((evt) => {
        $.ajax({
            type: 'GET',
            url: 'Application',
            success: function (data) {
                applicationData = [...data];
                uiEvents.RenderUI(applicationData);
                console.log(data);
            },
            error: function (err) {
                console.log("Error in loading applications.!!!");
            },
            contentType: "application/json",
            dataType: 'json'
        });
    });

    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#btnRegister").click(function () {
        const registerData = {
            "Name": $("#name").val(),
            "EmailId": $("#email").val(),
            "UserName": $("#username").val(),
            "Password": $("#password").val(),
            "Role": $("#role").val()
        };

        if (registerData.Name && registerData.EmailId && registerData.UserName && registerData.Password && registerData.Role) {
            if (ValidateEmail()) {
                $.ajax({
                    type: 'POST',
                    url: 'RegisterUser',
                    data: JSON.stringify(registerData),
                    success: function (data) {
                        ClearRegister();
                        alert(`Registered Successfully : ${data.Id}!`);
                    },
                    error: function (err) {
                        $("#username").val("");
                        $("#username").focus();
                        alert(registerData.UserName + " ..!!! User name already exists");
                    },
                    contentType: "application/json",
                    dataType: 'json'
                });
            } else {
                alert("Please enter valid email id.!!!");
            }
        }
        else {
            alert("Please select all information to proceed.!!!")
        }
    });

    function ClearRegister() {
        $("#name").val("");
        $("#email").val("");
        $("#username").val("");
        $("#password").val("");
        $("#role").val("-1");
    }

    function ClearApplication() {
        $("#add-app-name").val(""),
        $("#add-app-description").val("");
        $("#add-app-version").val("");
        $("#add-app-plannedRelease").val("");
        $("#add-app-actualRelease").val("");
        $("#add-app-environment").val("");
        $("#add-app-client").val("");
        $("#add-app-server").val("");
        $("#add-app-technology").val("");
        $("#add-app-type").val("");
    }

    function ValidateEmail(mail) {
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
            return (true)
        };
        return (false)
    }

    $("#btnSignOut").click(function () {
        window.location.href = '/Home/Login';
    });

    $("#deleteApplicationConfirmation").click((evt) => {
        $.ajax({
            type: 'GET',
            url: 'DeleteApplication?id=' + currentApplicationId,
            success: function (data) {
                applicationData = applicationData.filter(data => data.Id != currentApplicationId);
                uiEvents.RenderUI(applicationData);
                document.getElementsByClassName("closeClass")[0].click();
            },
            error: function (err) {
                alert("Error in deleting.!!!")
            },
            contentType: "application/json",
            dataType: 'json'
        });
    });

    $("#btnLogin").click(function () {
        const userName = $("#Lusername").val(),
            password = $("#Lpassword").val(),
            loginData = {
                "UserName": userName,
                "Password": password
            };

        if (loginData.UserName && loginData.Password) {
            $.ajax({
                type: 'POST',
                url: 'CheckLogin',
                data: JSON.stringify(loginData),
                success: function (data) {
                    userDetails = loginData;
                    if (data) {
                        userDetails = data;
                        window.location.href = '/Home/Main?userName=' + loginData.UserName;
                    }
                    else
                        alert(`Login failed.! Try again`)
                },
                error: function (err) {
                    alert("Error in login.!!!")
                },
                contentType: "application/json",
                dataType: 'json'
            });
        }
        else {
            alert("Please enter username and password to proceed.!!!")
        }
    });
});

function viewApplication(evt) {
    let currentApplicationId = evt;
    const currentApplicationDetails = applicationData.find(data => data.Id == currentApplicationId);
    $("#app-name").val(currentApplicationDetails.Name);
    $("#app-description").val(currentApplicationDetails.Description);
    $("#app-client").val(currentApplicationDetails.Client);
    $("#app-actualRelease").val(GetDate(currentApplicationDetails.ActualRelease));
    $("#app-createdBy").val(currentApplicationDetails.CreatedBy);
    $("#app-createdDate").val(GetDate(currentApplicationDetails.CreatedDate));
    $("#app-environment").val(currentApplicationDetails.Environment);
    $("#app-plannedRelease").val(GetDate(currentApplicationDetails.CreatedDate));
    $("#app-provider").val(currentApplicationDetails.Provider);
    $("#app-server").val(currentApplicationDetails.Server);
    $("#app-technology").val(currentApplicationDetails.Technology);
    $("#app-type").val(currentApplicationDetails.Type);
    $("#app-version").val(currentApplicationDetails.Version);
}

function GetDate(formatedDate) {
    let currentDate = new Date(+formatedDate.replace(/\D/g, ''));
    return `${currentDate.getUTCFullYear()}-${currentDate.getUTCMonth()}-${currentDate.getUTCDate()}`;

}

function deleteApplicationDOM(evt) {
    currentApplicationId = evt;
}

function UIEvents() {
    this.RenderUI = (datas) => {
        let tableBody = document.getElementById("myTable");
        let userName,
            isAdmin;
        if (userDetails) {
            userName = userDetails.UserName;
            isAdmin = userDetails.Role;
        }
        else {
            userName = window.location.href.split("=")[1];
            isAdmin = ($("#applicationHeader").children()[3].className == "employee") ? true : false;
        }

        $(tableBody).html("");
        datas.forEach((data) => {
            let template = `<tr>
            <td>${ data.Name}</td>
            <td>${ data.Version}</td>
            <td>
                <p data-placement="top" data-toggle="tooltip" title="Edit">
                    <button class="btn btn-primary btn-xs" onClick="viewApplication(${ data.Id})"
                        id="${ "application_edit_" + data.Id}" 
                        data-title="Edit" 
                        data-toggle="modal" data-target="#edit">
                        <span class="glyphicon glyphicon-list-alt"></span>
                    </button>
                </p>
            </td>
            <td class="${(isAdmin ? "employee" : "admin")}">
                <p data-placement="top" data-toggle="tooltip" title="Delete">
                    <button class="btn btn-danger btn-xs"
                        onClick="deleteApplicationDOM(${ data.Id})"
                        id="${ "application_delete_" + data.Id}" 
                        data-title="Delete" 
                        data-toggle="modal" 
                        data-target="#delete">
                        <span class="glyphicon glyphicon-trash"></span>
                    </button>
                </p>
            </td>
        </tr>`;

            $(tableBody).append(template);
        });
        console.log("RenderUI");
    }
}