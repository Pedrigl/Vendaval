const PROXY_CONFIG = [
  {
    context: [
      "/Home",
    ],
    target: "https://localhost:8000",
    secure: false,
    ws: true
  }
]

module.exports = PROXY_CONFIG;
