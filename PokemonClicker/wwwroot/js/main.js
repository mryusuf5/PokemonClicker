const pokeBall = document.querySelector("#pokeBall");
const counter = document.querySelector("#counter");
const hiddenpoints = document.querySelector("#playerPointsHidden");
const closeBattlePokemonShopModal = document.querySelector("#closeBattlePokemonShopModal");

pokeBall.addEventListener("click", () => {
    let count = counter.innerHTML;
    count++;
    counter.innerHTML = count;
    hiddenpoints.value = count;
})

const getPokemons = async () =>{
    // const api = await fetch("https://pokeapi.co/api/v2/pokemon/?limit=135&offset=251");
    const api = await fetch("https://pokeapi.co/api/v2/pokemon/?limit=1170");
    const data = await api.json();
    return data;
}

const getSinglePokemonData = async (pokemonName) => {
    const api = await fetch(`https://pokeapi.co/api/v2/pokemon/${pokemonName}`)
    const data = await api.json();
    
    return data;
}

const getPokemonCatchRate = async (pokemonName) => {
    const api = await fetch(`https://pokeapi.co/api/v2/pokemon-species/${pokemonName}`)
    const data = await api.json();

    return data;
}

const displayPokemon = async () => {
    let pokemons = await getPokemons();
    let pokemonsArray = [];
    if(!localStorage.getItem("pokemons"))
    {
        for (const pokemon of pokemons.results) {

            let singlePokemon = await getSinglePokemonData(pokemon.name)
            let filteredName = pokemon.name.includes("mega") || pokemon.name.includes("pikachu") || pokemon.name.includes("deoxys") || pokemon.name.includes("alola") || pokemon.name.includes("galar") || pokemon.name.includes("greninja") || pokemon.name.includes("zygarde") || pokemon.name.includes("floette") || pokemon.name.includes("totem") || pokemon.name.includes("oricorio") || pokemon.name.includes("ycanroc") || pokemon.name.includes("wishiwashi") || pokemon.name.includes("minior") || pokemon.name == "wormadam-plant" || pokemon.name == "giratina-altered" || pokemon.name == "shaymin-land" || pokemon.name == "basculin-red-striped" || pokemon.name == "darmanitan-standard" || pokemon.name == "tornadus-incarnate" || pokemon.name == "thundurus-incarnate" || pokemon.name == "landorus-incarnate" || pokemon.name == "keldeo-ordinary" || pokemon.name == "meloetta-aria" || pokemon.name == "meowstic-male" || pokemon.name == "aegislash-shield" || pokemon.name == "pumpkaboo-average" || pokemon.name == "gourgeist-average" || pokemon.name == "lycanroc-midday" || pokemon.name == "wishiwashi-solo" || pokemon.name == "mimikyu-disguised" || pokemon.name == "toxtricity-amped" || pokemon.name == "eiscue-ice" || pokemon.name == "indeedee-male" || pokemon.name == "morpeko-full-belly" || pokemon.name == "urshifu-single-strike" || pokemon.name == "basculegion-male" || pokemon.name == "enamorus-incarnate" || pokemon.name == "wormadam-sandy" || pokemon.name == "wormadam-trash" || pokemon.name == "shaymin-sky" || pokemon.name == "giratina-origin" || pokemon.name == "rotom-heat" || pokemon.name == "rotom-frost" || pokemon.name == "rotom-wash" || pokemon.name == "rotom-fan" || pokemon.name == "rotom-mow" || pokemon.name == "castform-sunny" || pokemon.name == "castform-rainy" || pokemon.name == "castform-snowy" || pokemon.name == "basculin-blue-striped" || pokemon.name == "darmanitan-zen" || pokemon.name == "meloetta-pirouette" || pokemon.name == "tornadus-therian" || pokemon.name == "thundurus-therian" || pokemon.name == "landorus-therian" || pokemon.name == "kyurem-black" || pokemon.name == "kyurem-white" || pokemon.name == "keldeo-resolute" || pokemon.name == "meowstic-female" || pokemon.name == "aegislash-blade" || pokemon.name == "pumpkaboo-small" || pokemon.name == "pumpkaboo-large" || pokemon.name == "pumpkaboo-super" || pokemon.name == "gourgeist-small" || pokemon.name == "gourgeist-large" || pokemon.name == "gourgeist-super" || pokemon.name == "groudon-primal" || pokemon.name == "kyogre-primal" || pokemon.name == "hoopa-unbound" || pokemon.name == "mimikyu-busted" ? pokemon.name.split("-")[0] : pokemon.name;
            let singlePokemonSpecies = await getPokemonCatchRate(filteredName);

            let totalStats = 0;

            singlePokemon.stats.forEach((stat) => {
                totalStats += stat.base_stat;
            })

            pokemon.image = singlePokemon.sprites.other.showdown.front_default ? singlePokemon.sprites.other.showdown.front_default : singlePokemon.sprites.front_default;
            pokemon.totalStats = totalStats;
            pokemon.captureRate = singlePokemonSpecies.capture_rate;

            pokemonsArray.push(pokemon)
        }
        
        localStorage.setItem("pokemons", JSON.stringify(pokemonsArray));
        
        $("#pokemonStoreContainer").remove(".lds-ring")
    }
    
    let pokemonsFromStorage = JSON.parse(localStorage.getItem("pokemons"));
    
    pokemonsFromStorage.forEach((pokemon) => {
        $("#pokemonStoreContainer").append(`
            <div class="card col-sm-3 col-10">
                <div class="card-title">
                    <h4>${pokemon.name}</h4>
                </div>
                <div class="card-body">
                    <img style="object-fit: contain; height: 100%; width: 100%" src="${pokemon.image}">
                </div>
                <div class="card-footer">
                    <button data-bs-toggle="modal" data-bs-target="#battlePokemonModal" 
                    class="btn btn-primary w-100 d-flex justify-content-between battlePokemonButton"
                    data-pokemon-image="${pokemon.image}" data-pokemon-health="${pokemon.totalStats}" data-pokemon-name="${pokemon.name}"
                    data-pokemon-catch-rate="${pokemon.captureRate}"
                    >
                        <span>Battle</span>
                        <span class="d-flex align-items-center">
                            HP: &nbsp;${pokemon.totalStats}
                        </span>
                    </button>
                </div>
            </div>
        `);
    })
}

displayPokemon().then(() => {
    const battlePokemonButtons = document.querySelectorAll(".battlePokemonButton");
    const battlePokemonImage = document.querySelector("#battlePokemonImage");
    const pokemonHealth = document.querySelector("#pokemonHealth");
    const pokemonHealthContainer = document.querySelector("#pokemonHealthContainer");
    let pokemonBattleTimer = document.querySelector("#pokemonBattleTimer");
    const throwablePokeballs = document.querySelector("#throwablePokeballs");
    const pokemonLevelElement = document.querySelector("#pokemonLevel");
    
    let pokemonFullHealth;
    let timerRunning = true;
    
    battlePokemonButtons.forEach((button) => {
        button.addEventListener("click", (e) => {
            setTimeout(() => {
                let pokemonLevel = Math.floor(Math.random() * 100) + 5;
                pokemonLevelElement.textContent = "Level: " + pokemonLevel;
                closeBattlePokemonShopModal.click();
                battlePokemonImage.src = button.dataset.pokemonImage;
                let totalWidth = Math.round(button.dataset.pokemonHealth) + 2;
                pokemonFullHealth = button.dataset.pokemonHealth;
                pokemonHealth.style.width = button.dataset.pokemonHealth +"px"
                pokemonHealthContainer.style.width = totalWidth +"px"               

                let pokemonTotalStat = document.querySelectorAll(".battlePokemonStats");

                pokemonTotalStat.forEach((input) => {
                    input.value = pokemonFullHealth;
                })
                
                let intervalTimer = setInterval(() => {
                    let time = pokemonBattleTimer.innerHTML;
                    time--;
                    pokemonBattleTimer.innerHTML = time;
                    if(time == 0)
                    {
                        alert("Too bad, you lost :(");
                        window.location.reload();
                    }
                }, 1000)

                battlePokemonImage.addEventListener("click", () => {
                    pokemonFullHealth--;
                    pokemonHealth.style.width = pokemonFullHealth + "px";
                    if(pokemonFullHealth == 0)
                    {
                        clearInterval(intervalTimer);
                        throwablePokeballs.classList.remove("d-none");
                        throwablePokeballs.classList.add("d-flex")
                        document.querySelectorAll(".battlePokemonName").forEach((input) => {
                            input.value = button.dataset.pokemonName;
                        })
                        document.querySelectorAll(".battlePokemonImageInput").forEach((input) => {
                            input.value = button.dataset.pokemonImage;
                        })
                        document.querySelectorAll(".battlePokemonCatchRate").forEach((input) => {
                            input.value = button.dataset.pokemonCatchRate;
                        })
                        document.querySelectorAll(".battlePokemonLevel").forEach((input) => {
                            input.value = pokemonLevel
                        })
                        
                        alert("Congrats!");
                    }
                })
            })
        })
    })
});