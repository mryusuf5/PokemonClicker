const pokeBall = document.querySelector("#pokeBall");
const counter = document.querySelector("#counter");
const hiddenpoints = document.querySelector("#playerPointsHidden");

pokeBall.addEventListener("click", () => {
    let count = counter.innerHTML;
    count++;
    counter.innerHTML = count;
    hiddenpoints.value = count;
})