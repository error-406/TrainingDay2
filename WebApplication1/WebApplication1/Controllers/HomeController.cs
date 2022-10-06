using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyGlobalVariable _MyGlobalVariable;

        public HomeController(ILogger<HomeController> logger, IOptions<MyGlobalVariable> MyGlobalVariable)
        {
            _logger = logger;
            _MyGlobalVariable = MyGlobalVariable.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PartialView1()
        {
            ViewBag.Data = "Value from Controller";
            return PartialView();
        }

            public IActionResult GetListUser()
        {
            List<LoginUser> loginuser = new List<LoginUser>();
            string constr = _MyGlobalVariable.MySqlConString;
            if (constr != null)
            {
                string query = "SELECT ID, username, password FROM loginuser;";
                using (MySqlConnection con = new MySqlConnection(constr))
                { 
                    using(MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                loginuser.Add(new LoginUser {
                                    Id = Convert.ToInt64(sdr["ID"]),
                                    Username = sdr["username"].ToString(),
                                    Password = sdr["password"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            return View(loginuser);
        }

        public IActionResult CreateUser()
        {
            return View();
        }
        //
        public IActionResult CreateUserSubmit(LoginUser usr)
        {
            if (ModelState.IsValid)
            {
                string constr = _MyGlobalVariable.MySqlConString;
                if (constr != null)
                {
                    string query = "INSERT INTO loginuser (username,password) VALUES (@username, @password);";
                    using (MySqlConnection con = new MySqlConnection(constr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.Parameters.AddWithValue("@username", usr.Username);
                            cmd.Parameters.AddWithValue("@password", usr.Password);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            return RedirectToAction("GetListUser", "Home");
        }

        public IActionResult UpdateUser(Int64 id)
        {
            LoginUser loginuser = new LoginUser();
            string constr = _MyGlobalVariable.MySqlConString;
            if (constr != null)
            {
                string query = "SELECT ID, username, password FROM loginuser WHERE ID = "+ id +" LIMIT 1;";
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                loginuser.Id = Convert.ToInt64(sdr["ID"]);
                                loginuser.Username = sdr["username"].ToString();
                                loginuser.Password = sdr["password"].ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return View(loginuser);
        }

        //ActionUpdateUser
        public IActionResult ActionUpdateUser(LoginUser usr)
        {
            if (ModelState.IsValid)
            {
                string constr = _MyGlobalVariable.MySqlConString;
                if (constr != null)
                {
                    string query = "UPDATE loginuser SET username = @username , password = @password WHERE Id = @Id;";
                    using (MySqlConnection con = new MySqlConnection(constr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.Parameters.AddWithValue("@username", usr.Username);
                            cmd.Parameters.AddWithValue("@password", usr.Password);
                            cmd.Parameters.AddWithValue("@id", usr.Id);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            return RedirectToAction("GetListUser", "Home");
        }

        //DeleteUser
        public IActionResult DeleteUser(Int64 id)
        {
            LoginUser loginuser = new LoginUser();
            string constr = _MyGlobalVariable.MySqlConString;
            if (constr != null)
            {
                string query = "SELECT ID, username, password FROM loginuser WHERE ID = " + id + " LIMIT 1;";
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                loginuser.Id = Convert.ToInt64(sdr["ID"]);
                                loginuser.Username = sdr["username"].ToString();
                                loginuser.Password = sdr["password"].ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }
            return View(loginuser);
        }
        public IActionResult ActionDeleteUser(LoginUser usr)
        {
            if (usr.Id != 0)
            {
                string constr = _MyGlobalVariable.MySqlConString;
                if (constr != null)
                {
                    string query = "DELETE FROM loginuser WHERE Id = @Id;";
                    using (MySqlConnection con = new MySqlConnection(constr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.Parameters.AddWithValue("@id", usr.Id);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            return RedirectToAction("GetListUser", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}