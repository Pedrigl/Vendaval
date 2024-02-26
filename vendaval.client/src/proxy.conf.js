const PROXY_CONFIG = [
  {
    context: [
      "/Home",
    ],
    target: "https://localhost:7118",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
