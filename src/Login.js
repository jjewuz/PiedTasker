import { LoginButton } from '@telegram-auth/react';
import { useNavigate } from "react-router";

function Login() {
    let navigate = useNavigate();

    return (
        <div className="container">
        <h1>PiedTracker ALPHA Login Page</h1>
        <div className="background"></div>
        <div className="tg-login-button">
                  <LoginButton
                      botUsername={process.env.REACT_APP_BOT_USERNAME}
                      onAuthCallback={(data) => {
                        const userId = data.id;
                        localStorage.setItem("tg_id", userId);
                          console.log(data); //TODO Передаём дату в бек и там обрабатываем
                        navigate("/"); 
                      }}
                      lang='ru'
                  />
        </div>
      </div>
    );
  };

export default Login;