$(function () {
    EmpRequest.GetEmployees(1, DocManager.SetUpEmployees, DocManager.SetUpPages);
    $("#add").click(function (event) {
        event.preventDefault();
        EmpRequest.GetCreate(DocManager.SetUpSelections);
    });
    $("#aboutPage").click(function (event) {
    });
});
var Position = /** @class */ (function () {
    function Position() {
    }
    return Position;
}());
var Department = /** @class */ (function () {
    function Department() {
    }
    return Department;
}());
var Employee = /** @class */ (function () {
    function Employee() {
    }
    return Employee;
}());
var EmpRequest = /** @class */ (function () {
    function EmpRequest() {
    }
    EmpRequest.GetAbout = function (f) {
        $("");
    };
    EmpRequest.GetEdit = function (f, id) {
        console.log("GET request for Edit()");
        $("#appearingLayout").load("Employees/Edit/", function () {
            console.log("GET request for GetSelections()");
            $.getJSON("Employees/GetSelections/", function (data) {
                DocManager.SetUpSelections(data, id);
            }).done(function () {
                console.log("GET succeed");
                console.log("GET request for GetEmployee()");
                $.getJSON("Employees/GetEmployee/" + id).done(function (response) {
                    console.log("GET succeed");
                    $("#cancel").click(function (event) {
                        event.preventDefault();
                        DocManager.RemoveAppearingHtml();
                    });
                    $("#edit").click(function (event) {
                        event.preventDefault();
                        EmpRequest.PostEdit(id);
                    });
                    f(response.employee, id);
                }).fail(function (data) {
                    console.log("GET failed");
                });
            }).fail(function () {
                console.log("GET failed");
            });
        });
    };
    EmpRequest.PostEdit = function (id) {
        console.log("POST to EditConfirmed");
        var emp = new Employee();
        var bossSelect = document.getElementById("bossSelection");
        var posSelect = document.getElementById("positionSelection");
        var depSelect = document.getElementById("departmentSelection");
        emp.Id = Number(id);
        emp.Name = document.getElementById("name").value;
        emp.Secondname = document.getElementById("secondname").value;
        emp.Surname = document.getElementById("surname").value;
        emp.Boss = new Employee();
        if (bossSelect.selectedIndex < 0) {
            emp.BossId = null;
            emp.Boss.Id = emp.BossId;
            emp.Boss.Surname = "";
        }
        else {
            emp.BossId = Number(bossSelect[bossSelect.selectedIndex].value);
            emp.Boss.Id = emp.BossId;
            emp.Boss.Surname = bossSelect[bossSelect.selectedIndex].textContent;
        }
        emp.PositionId = Number(posSelect[posSelect.selectedIndex].value);
        emp.DepartmentId = Number(depSelect[depSelect.selectedIndex].value);
        emp.Position = new Position();
        emp.Position.Id = emp.PositionId;
        emp.Position.Name = posSelect[posSelect.selectedIndex].textContent;
        emp.Department = new Department();
        emp.Department.Id = emp.DepartmentId;
        emp.Department.Name = depSelect[depSelect.selectedIndex].textContent;
        var date = new Date(document.getElementById("recdate").value);
        emp.RecruitDate = date.toISOString();
        $.ajax({
            type: "POST",
            url: "Employees/EditConfirmed/",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(emp),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    DocManager.RemoveAppearingHtml();
                    DocManager.UpdateRow(id, response.employee);
                }
                else {
                    alert("Сервер не смог обработать запрос, " +
                        "возможно, перезагрузка страницы cможет помочь");
                    console.log("Controller returned error");
                }
            },
            error: function (response) {
                console.log("POST failed");
            }
        });
    };
    EmpRequest.GetEmployees = function (pageNum, setUpEmp, setUpPages) {
        console.log("GET request for GetEmployees()");
        $.getJSON("/Employees/GetEmployees/" + pageNum).done(function (response) {
            if (response.success) {
                console.log("GET succeed");
                setUpEmp(response.employees);
            }
            else {
                console.log("GET succeed");
            }
        }).fail(function (response) {
            console.log("GET failed");
        });
        //$.ajax({
        //    type: "POST",
        //    url: "/Employees/GetEmployees/",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    data: JSON.stringify(pageNum),
        //    success: function (response) {
        //        if (response.success) {
        //            console.log("POST succeed");
        //            setUpEmp(response.employees);
        //        } else {
        //            console.log("POST succeed");
        //        }
        //    },
        //    error: function (response) {
        //        console.log("POST failed");
        //    }
        //});
    };
    EmpRequest.GetBosses = function (f, ref) {
        console.log("GET request for GetBosses()");
        $.getJSON(ref, function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            f();
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.GetCreate = function (f) {
        console.log("GET request for GetSelections()");
        $.getJSON("Employees/GetSelections/", function (data) {
        }).done(function (data) {
            console.log("GET succeed");
            $("#appearingLayout").load("Employees/Create", function () {
                $("#cancel").click(function (event) {
                    event.preventDefault();
                    DocManager.RemoveAppearingHtml();
                });
                $("#create").click(function (event) {
                    event.preventDefault();
                    EmpRequest.PostCreate();
                });
                f(data);
            });
        }).fail(function (data) {
            console.log("GET failed");
        });
    };
    EmpRequest.PostCreate = function () {
        console.log("POST to Create");
        var emp = new Employee();
        var bossSelect = document.getElementById("bossSelection");
        var posSelect = document.getElementById("positionSelection");
        var depSelect = document.getElementById("departmentSelection");
        emp.Id = null;
        emp.Name = document.getElementById("name").value;
        emp.Secondname = document.getElementById("secondname").value;
        emp.Surname = document.getElementById("surname").value;
        emp.BossId = Number(bossSelect[bossSelect.selectedIndex].value);
        emp.PositionId = Number(posSelect[posSelect.selectedIndex].value);
        emp.DepartmentId = Number(depSelect[depSelect.selectedIndex].value);
        emp.Boss = null;
        emp.Position = null;
        emp.Department = null;
        var date = new Date(document.getElementById("recdate").value);
        emp.RecruitDate = date.toISOString();
        $.ajax({
            type: "POST",
            url: "Employees/Create/",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(emp),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    DocManager.RemoveAppearingHtml();
                    DocManager.AddRow(response.employee);
                    //TODO: не вставлять, если не на той странице?
                }
                else {
                    alert("Сервер не смог обработать запрос, " +
                        "возможно, перезагрузка страницы cможет помочь");
                    console.log("Controller returned error");
                }
            },
            error: function (response) {
                console.log("POST failed");
            }
        });
    };
    EmpRequest.GetDelete = function (f, id) {
        console.log("GET request for Delete()");
        $("#appearingLayout").load("Employees/Delete/", function () {
            console.log("GET succeed");
            $("#cancel").click(function (event) {
                event.preventDefault();
                DocManager.RemoveAppearingHtml();
            });
            $("#delete").click(function (event) {
                event.preventDefault();
                EmpRequest.PostDelete(id);
            });
            f(id);
        });
    };
    EmpRequest.PostDelete = function (id) {
        console.log("POST to Delete()");
        $.ajax({
            type: "POST",
            url: "Employees/DeleteConfirmed/" + id,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(id),
            success: function (response) {
                if (response.success) {
                    console.log("POST succeed");
                    DocManager.RemoveAppearingHtml();
                    DocManager.RemoveRow(id);
                }
                else {
                    alert("Сервер не смог обработать запрос, " +
                        "возможно, перезагрузка страницы cможет помочь");
                    console.log("Controller returned error");
                }
            },
            error: function (response) {
                console.log("POST failed");
            }
        });
    };
    EmpRequest.tableHeaders = [
        "Имя",
        "Отчество",
        "Фамилия",
        "Должность",
        "Отдел",
        "Руководитель",
        "Дата трудоустройства",
        "Действия",
    ];
    return EmpRequest;
}());
var DocManager = /** @class */ (function () {
    function DocManager() {
    }
    DocManager.RemoveAppearingHtml = function () {
        document.getElementById("appearingLayout").innerHTML = "";
    };
    DocManager.SetUpSelections = function (data, id) {
        if (id === void 0) { id = null; }
        var bosses = data["bosses"];
        var positions = data["positions"];
        var departments = data["departments"];
        for (var i = 0; i < bosses.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(bosses[i].id);
            opt.textContent = bosses[i].surname;
            $("#bossSelection").append(opt);
        }
        for (var i = 0; i < positions.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(positions[i].id);
            opt.textContent = positions[i].name;
            $("#positionSelection").append(opt);
        }
        for (var i = 0; i < departments.length; ++i) {
            var opt = document.createElement("option");
            opt.value = String(departments[i].id);
            opt.textContent = departments[i].name;
            $("#departmentSelection").append(opt);
        }
        $("#recdate").val(DocManager.ToHTMLDate(new Date()));
    };
    DocManager.SetUpBosses = function () {
    };
    DocManager.SetUpDelete = function (id) {
        var children = $("#" + id).children();
        var name = children[0].textContent;
        var secondname = children[1].textContent;
        var surname = children[2].textContent;
        var pos = children[3].textContent;
        var dep = children[4].textContent;
        $("#info").text(name + " " + secondname + " " + surname + " ( " + dep + ", " + pos + ")");
    };
    DocManager.SetUpEdit = function (emp) {
        $("#name").val(emp.name);
        $("#secondname").val(emp.secondname);
        $("#surname").val(emp.surname);
        $("#bossSelection").val(emp.bossId);
        $("#positionSelection").val(emp.positionId);
        $("#departmentSelection").val(emp.departmentId);
        $("#recdate").val(DocManager.ToHTMLDate(new Date(emp.recruitDate)));
    };
    DocManager.SetUpEmployees = function (data) {
        var table = document.createElement("table");
        table.className = "tablestyle";
        var thead = document.createElement("thead");
        thead.className = "theadstyle";
        var tr = document.createElement("tr");
        tr.className = "trstyle";
        for (var i = 0; i < EmpRequest.tableHeaders.length; ++i) {
            var th = document.createElement("th");
            th.className = "thstyle";
            th.textContent = EmpRequest.tableHeaders[i];
            tr.appendChild(th);
        }
        thead.appendChild(tr);
        table.appendChild(thead);
        var tbody = document.createElement("tbody");
        tbody.id = "tableBody";
        for (var i = 0; i < data.length; ++i) {
            DocManager.AddRow(data[i], tbody);
        }
        table.appendChild(tbody);
        $("#empTable").html(table);
    };
    DocManager.UpdateRow = function (id, emp) {
        var children = $("#" + id).children();
        children[0].textContent = emp.name;
        children[1].textContent = emp.secondname;
        children[2].textContent = emp.surname;
        children[3].textContent = emp.position.name;
        children[4].textContent = emp.department.name;
        children[5].textContent = emp.boss.surname;
        children[6].textContent = DocManager.ToReadableDate(new Date(emp.recruitDate));
    };
    DocManager.RemoveRow = function (id) {
        $("#" + id).remove();
    };
    DocManager.AddRow = function (emp, tbody) {
        if (tbody === void 0) { tbody = null; }
        var tr = document.createElement("tr");
        tr.id = String(emp.id);
        var td1 = document.createElement("td");
        td1.className = "tdstyle";
        td1.textContent = emp.name;
        var td2 = document.createElement("td");
        td2.className = "tdstyle";
        td2.textContent = emp.secondname;
        var td3 = document.createElement("td");
        td3.className = "tdstyle";
        td3.textContent = emp.surname;
        var td4 = document.createElement("td");
        td4.className = "tdstyle";
        td4.textContent = emp.position.name;
        var td5 = document.createElement("td");
        td5.className = "tdstyle";
        td5.textContent = emp.department.name;
        if (emp.boss.surname == null) {
            emp.boss.surname = "";
        }
        var td6 = document.createElement("td");
        td6.className = "tdstyle";
        td6.textContent = emp.boss.surname;
        var td7 = document.createElement("td");
        td7.className = "tdstyle";
        td7.textContent = DocManager.ToReadableDate(new Date(emp.recruitDate));
        var td8 = document.createElement("td");
        td8.className = "tdstyle";
        var a1 = document.createElement("a");
        a1.className = "editButton";
        a1.textContent = "Изменить";
        var id = emp.id;
        a1.addEventListener("click", function (e) {
            e.preventDefault();
            EmpRequest.GetEdit(DocManager.SetUpEdit, id);
        }, false);
        var a2 = document.createElement("a");
        a2.className = "bossesButton";
        a2.textContent = "Руководители";
        a2.addEventListener("click", function (e) {
            e.preventDefault();
            //TODO
        }, false);
        var a3 = document.createElement("a");
        a3.className = "deleteButton";
        a3.textContent = "Удалить";
        a3.addEventListener("click", function (e) {
            e.preventDefault();
            EmpRequest.GetDelete(DocManager.SetUpDelete, id);
        }, false);
        var tr21 = document.createElement("tr");
        var tr22 = document.createElement("tr");
        var tr23 = document.createElement("tr");
        var td21 = document.createElement("td");
        var td22 = document.createElement("td");
        var td23 = document.createElement("td");
        td21.appendChild(a1);
        td22.appendChild(a2);
        td23.appendChild(a3);
        tr21.appendChild(td21);
        tr22.appendChild(td22);
        tr23.appendChild(td23);
        td8.appendChild(tr21);
        td8.appendChild(tr22);
        td8.appendChild(tr23);
        tr.appendChild(td1);
        tr.appendChild(td2);
        tr.appendChild(td3);
        tr.appendChild(td4);
        tr.appendChild(td5);
        tr.appendChild(td6);
        tr.appendChild(td7);
        tr.appendChild(td8);
        if (tbody == null) {
            document.getElementById("tableBody").appendChild(tr);
        }
        else {
            tbody.appendChild(tr);
        }
    };
    DocManager.ToReadableDate = function (date) {
        var y = date.getFullYear();
        var m = String(date.getMonth() + 1);
        m = (m.length == 1) ?
            ("0" + m) : (m);
        var d = (String(date.getDate()).length == 1) ?
            ("0" + date.getDate()) : (date.getDate());
        return d + "." + m + "." + y;
    };
    DocManager.ToHTMLDate = function (date) {
        var y = date.getFullYear();
        var m = String(date.getMonth() + 1);
        m = (m.length == 1) ?
            ("0" + m) : (m);
        var d = (String(date.getDate()).length == 1) ?
            ("0" + date.getDate()) : (date.getDate());
        return y + "-" + m + "-" + d;
    };
    DocManager.SetUpPages = function () {
    };
    return DocManager;
}());
//# sourceMappingURL=app.js.map