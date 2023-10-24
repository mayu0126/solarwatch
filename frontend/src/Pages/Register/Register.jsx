import { useState } from "react";
//import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import CustomerForm from "../../Components/CustomerForm/CustomerForm.jsx";
import "./Register.css";

const createCustomer = (customer) => {
  console.log(customer);
  const url = process.env.REACT_APP_MY_URL;

  return fetch(`${url}/Auth/Register`, {
    method: "POST",
    mode: "cors",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(customer),
  }).then((res) => 
  {
    if (!res.ok) {
      return res.json().then((data) => {
        let errorMessage = "Registration failed";

        if (data) {
          if (data["DuplicateUserName"]) {
            errorMessage = data["DuplicateUserName"][0];
          } else if (data["DuplicateEmail"]) {
            errorMessage = data["DuplicateEmail"][0];
          } else if (data["PasswordTooShort"]) {
            errorMessage = data["PasswordTooShort"][0];
          }
        }

        throw new Error(errorMessage);
      });
    }
    return res.json(); //if the response is "ok"
  });
};

const Register = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handleCreateCustomer = (customer) => {
    console.log(customer)
    setLoading(true);
    createCustomer(customer)
      .then(() => {
        setLoading(false);
        navigate("/");
      })
      .catch((error) => {
        setLoading(false);
        console.error("Registration error:", error.message);
        setErrorMessage(error.message);
      });
  };

  return (
    <CustomerForm
      onCancel={() => navigate("/")}
      onSave={handleCreateCustomer}
      disabled={loading}
      isRegister={true}
      errorMessage={errorMessage}
    />
  );
};

export default Register;
