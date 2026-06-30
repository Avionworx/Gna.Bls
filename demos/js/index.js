async function getToken()
{   
    const url =
    document.getElementById("url").value.trim();

    const env =
    document.getElementById("env").value.trim();

    const email =
    document.getElementById("email").value.trim();

    const apiKey =
    document.getElementById("apiKey").value.trim();

    const authResponse =
    await fetch(
        `${url}/${env}/login?email=${email}`,
        {
            method: "POST",
            headers: {
                "X-API-KEY": apiKey
            }
        });

const jwt =
    await authResponse.text();

    return jwt;
}


async function runDemo() {

    try {

        const url =
        document.getElementById("url").value.trim();

        const env =
        document.getElementById("env").value.trim();

        const blueprint =
        document.getElementById("blueprint").value.trim() || "master"; 

        const jwt = await getToken();

        document.getElementById("result").textContent =
            "Authenticated. Running evaluation...";

        const payload =
            document.getElementById("payload").value;

        let lbls = document.getElementById("labels").value.trim();
        let accept = document.getElementById("acceptSelect").value;

        lbls = (lbls.split(",").map((item, i) => `label=${item}`)).join("&")

        const response =
            await fetch(
                `${url}/${env}/${blueprint}/evaluate?${lbls}`,
                {
                    method: "POST",
                    headers: {
                        "Authorization": `Bearer ${jwt}`,
                        "Content-Type": "application/json",
                        "Accept" :accept
                    },
                    body: payload
                });
                
        const contentType = response.headers.get("content-type");

        if (contentType?.includes("application/json")) {        
            const json =
                await response.json();
    
            document.getElementById("result").textContent =
                JSON.stringify(json, null, 2);
            }
            if (contentType?.includes("text/plain")) {        
                const text =
                    await response.text();
        
                document.getElementById("result").textContent = text;
            }           

    }
    catch (err) {

        document.getElementById("result").textContent =
            err.toString();
    }
}

function loadSettings() {
    document.getElementById("url").value =
        localStorage.getItem("bls_url") || "";

    document.getElementById("env").value =
        localStorage.getItem("bls_env") || "";

    document.getElementById("blueprint").value =
        localStorage.getItem("bls_blueprint");

    document.getElementById("apiKey").value =
        localStorage.getItem("bls_apiKey") || "";

    document.getElementById("email").value =
        localStorage.getItem("bls_email") || "";
}

function saveSettings() {
    localStorage.setItem(
        "bls_url",
        document.getElementById("url").value);

    localStorage.setItem(
        "bls_env",
        document.getElementById("env").value);

    localStorage.setItem(
        "bls_blueprint",
        document.getElementById("blueprint").value);

    localStorage.setItem(
        "bls_apiKey",
        document.getElementById("apiKey").value);

    localStorage.setItem(
        "bls_email",
        document.getElementById("email").value);
 
}

function clearSettings() {
    localStorage.removeItem("bls_url");
    localStorage.removeItem("bls_env");
    localStorage.removeItem("bls_blueprint");
    localStorage.removeItem("bls_apiKey");
    localStorage.removeItem("bls_email");

    loadSettings();
}

window.addEventListener("load", loadSettings);

document
.getElementById("saveSettingsBtn")
.addEventListener("click", saveSettings);
    
document
.getElementById("clearSettingsBtn")
.addEventListener("click", saveSettings);

document
.getElementById("runDemoButton")
.addEventListener("click", runDemo);  