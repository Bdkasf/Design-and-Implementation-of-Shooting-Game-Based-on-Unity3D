using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System;
using System.Text;

public class DatabaseManager : MonoBehaviour
{
    // 数据库连接相关变量
    private MySqlConnection connection;
    private string serverName = "localhost";
    private string dbName = "UnityGame";	// 数据库名
    private string userName = "root";		// 登录数据库的用户名
    private string password = "123456";		// 登录数据库的密码
    private string port = "3306";           // MySQL服务的端口号

    // 注册UI和登录UI
    public GameObject RegisterUI;
    public GameObject LoginUI;

    // 用户名输入框和密码输入框
    public InputField usernameInputField;
    public InputField passwordInputfield;

    // 注册消息和登录消息
    public Text registerMessage;
    public Text loginMessage;

    public GameObject enterGameButton;

    void Start()
    {
        enterGameButton.SetActive(false);
        // 初始化UI状态
        LoginUI.SetActive(true);
        RegisterUI.SetActive(false);

        // 连接数据库
        string connectionString = "Server=" + serverName + ";Database=" + dbName + ";Uid=" + userName
                                  + ";Pwd=" + password + ";Port=" + port + ";";
        connection = new MySqlConnection(connectionString);
        connection.Open();
        Debug.Log("连接数据库成功");
    }

    // 加密密码
    private static string HashPassword(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        StringBuilder hash = new StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }

    // 注册逻辑
    public void OnRegister()
    {
        // 从输入框获取用户名和密码
        string username = usernameInputField.text;
        //使用哈希进行加密
        string password = HashPassword(passwordInputfield.text);

        if (username == "" || password == "")
        {
            registerMessage.text = "账号或密码不能为空";
        }

        else
        {
            // 检查数据库中是否存在具有给定用户名的用户记录
            string query1 = "SELECT COUNT(*) FROM usersinfo WHERE username = @Username";
            MySqlCommand cmd1 = new MySqlCommand(query1, connection);
            cmd1.Parameters.AddWithValue("@Username", username);
            int count = Convert.ToInt32(cmd1.ExecuteScalar());

            // 根据查询结果提示用户
            if (count > 0)
            {
                Debug.Log("用户名已存在，请选择不同的用户名！");
                registerMessage.text = "用户名已存在，请选择不同的用户名！";
            }
            else
            {
                // 构造插入数据的SQL语句，并将用户名和密码赋值给参数
                string query2 = "INSERT INTO usersinfo(username, password) VALUES (@username, @password)";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.Parameters.AddWithValue("@username", username);
                cmd2.Parameters.AddWithValue("@password", password);

                // 执行SQL语句，获取影响的行数
                int rowsAffected = cmd2.ExecuteNonQuery();

                // 根据影响的行数给出注册成功或失败的消息
                if (rowsAffected > 0)
                {
                    Debug.Log("注册成功");
                    registerMessage.text = "注册成功";
                }
                else
                {
                    Debug.Log("注册失败");
                    registerMessage.text = "注册失败";
                }
            }
            //清空输入框
            usernameInputField.text = "";
            passwordInputfield.text = "";
        }
    }


	// 登录逻辑
    public void OnLogin()
    {
        // 从输入框获取用户名和密码
        string username = usernameInputField.text;
        //使用哈希进行加密
        string password = HashPassword(passwordInputfield.text);

        if (username == "" || password == "")
        {
            loginMessage.text = "账号或密码不能为空";
        }
        else
        {
            // 构造查询数据的SQL语句，并将用户名和密码赋值给参数
            string query = "SELECT COUNT(*) FROM usersinfo WHERE username=@username AND password=@password";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            // 执行SQL语句，获取查询结果
            object result = cmd.ExecuteScalar();
            int count = Convert.ToInt32(result);

            // 根据影响的行数给出注册成功或失败的消息，并清空输入框
            if (count > 0)
            {
                Debug.Log("登录成功");
                loginMessage.text = "登录成功";
                enterGameButton.SetActive(true); // 显示"进入游戏"按钮
            }
            else
            {
                // 根据查询结果给出不同的提示消息
                string errorMessage;
                query = "SELECT COUNT(*) FROM usersinfo WHERE username=@username";
                cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                result = cmd.ExecuteScalar();
                count = Convert.ToInt32(result);
                if (count == 0)
                {
                    errorMessage = "用户名不存在";
                    enterGameButton.SetActive(false);
                }
                else
                {
                    errorMessage = "密码错误";
                    enterGameButton.SetActive(false);
                }

                Debug.Log("登录失败：" + errorMessage);
                loginMessage.text = errorMessage;
                enterGameButton.SetActive(false); // 隐藏"进入游戏"按钮
            }
            usernameInputField.text = "";
            passwordInputfield.text = "";
        }
    }

}
