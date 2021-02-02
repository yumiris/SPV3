#include "send-presence.h"

struct char32 {
	char str[32];
};

struct maps {
	const char *mapfile_name;
	const char *map_name;
};

const maps SP_MAPS[] = {
	//Instalation 04
	{ "spv3a05", "Discovery" },
	{ "spv3a10", "The Pillar of Autumn" },
	{ "spv3a30", "Halo" },
	{ "spv3a50", "The Truth and Reconciliation" },
	{ "spv3b30", "The Silent Cartographer" },
	{ "spv3b30_evolved", "The Silent Cartographer: Evolved" },
	{ "spv3b40", "Assault on the Control Room" },
	{ "spv3c10", "343 Guilty Spark" },
	{ "spv3c20", "The Library" },
	{ "spv3c40", "Two Betrayals" },
	{ "spv3d20", "Keyes" },
	{ "spv3d25", "None Left Behind" },
	{ "spv3d30", "The Commander" },
	{ "spv3d30_evolved", "The Commander: Evolved" },
	{ "spv3d40", "The Maw" },
	//Lumoria
	{ "spv3_lumoria_a", "Lumoria Landfall" },
	{ "spv3_lumoria_b", "Valley Battle" },
	{ "spv3_lumoria_cd", "Fight for the Future" },
	//Firefight
	{ "spv3_rockslide_spartan", "Rockslide (UNSC/Covenant)" },
	{ "spv3_rockslide_flood",   "Rockslide (UNSC/Flood)" },
	{ "spv3_rockslide_elite",   "Rockslide (Covenant/Flood)" },
	{ "spv3_engine_spartan",    "Engine Room (UNSC/Covenant)" },
  { "spv3_engine_flood",      "Engine Room (UNSC/Flood)" },
	{ "spv3_engine_elite",      "Engine Room (Covenant/Flood)" },
};

const char *DIFFICULTIES[] = {
	"Noble",
	"Normal",
	"Heroic",
	"Legendary"
};

const char *Difficulties[] = {
	"noble",
	"normal",
	"heroic",
	"legendary"
};

HMODULE self_module;
HANDLE main_thread;


std::string combined;
int64_t start_time;
byte CURRENT_PLAYERS;
void main() {

	discordInit();
	combined = "Main Menu";;
	start_time = time(0);
	updateDiscordPresence(start_time, NULL, combined.c_str(), "ui", "", combined.c_str(), NULL);

	bool *game_started = (bool*)(0x68CD88);
	while (*game_started == false) {
		Sleep(1000);
	}

	DWORD *tick = (DWORD*)(0x692E9C);

	while (true) {
			std::string temp_combined;

			char32 map_name32 = *(char32*)(0x643064);
			std::string map_name = map_name32.str;
			std::transform(map_name.begin(), map_name.end(), map_name.begin(), ::tolower);

			if (map_name == "ui") {
				DWORD *changing_settings = (DWORD*)(0x6AFF04);
				DWORD *widget_open = (DWORD*)(0x6B401C);

				
				if (*widget_open != 0) {
					if (*changing_settings != 0xFFFFFFFF) {
						temp_combined = "Changing game settings";
					}
					else {
						temp_combined = "Main Menu";
					}

					if (temp_combined != combined) {
						start_time = time(0);
						updateDiscordPresence(start_time, NULL, temp_combined.c_str(), "ui", "", temp_combined.c_str(), NULL);
						combined = temp_combined;
					}
				} else {
					temp_combined = "Main Menu";
					if (temp_combined != combined) {
						start_time = time(0);
						updateDiscordPresence(start_time, NULL, temp_combined.c_str(), "ui", "", temp_combined.c_str(), NULL);
						combined = temp_combined;
					}
				}
			} else if(*tick > 30) {
				std::string state = map_name32.str;

				bool in_server = *(bool*)(0x6B7352);
				if (in_server) {
					temp_combined = "naughty";
					if (temp_combined != combined) {
						start_time = time(0);
						updateDiscordPresence(start_time, NULL, "Doing very naughty things.", "naughty", "", "", NULL);
						combined = temp_combined;
					}
				} else {

					WORD difficulty = *(WORD*)(0x40000126);

					for (int i = 0; i < (sizeof(SP_MAPS) / sizeof(*SP_MAPS)); i++) {
						if (map_name == SP_MAPS[i].mapfile_name) {
							state = SP_MAPS[i].map_name;
							break;
						}
					}

					std::string difficulty_str;
					std::string real_difficulty;
					if (difficulty < 4) {
						real_difficulty = Difficulties[difficulty];
						difficulty_str = DIFFICULTIES[difficulty];
						difficulty_str += " Difficulty";
					}

					temp_combined += state.c_str();
					temp_combined += "sp";
					temp_combined += map_name.c_str();
					temp_combined += DIFFICULTIES[difficulty];

					if (temp_combined != combined) {
						start_time = time(0);
						updateDiscordPresence(start_time, difficulty_str.c_str(), state.c_str(), map_name.c_str(), real_difficulty.c_str(), state.c_str(), difficulty_str.c_str());
						combined = temp_combined;
					}
				}

			}
		Sleep(1000);
	}
}

bool APIENTRY DllMain(HINSTANCE hDLLInst, DWORD fdwReason, LPVOID lpvReserved) {
	if (fdwReason == DLL_PROCESS_ATTACH && main_thread == NULL) {
		self_module = hDLLInst;
		main_thread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)main, NULL, 0, NULL);
		if (main_thread == NULL) {
			return false;
		}
	}
	if (fdwReason == DLL_PROCESS_DETACH && main_thread != NULL) {
		TerminateThread(main_thread, 0);
	}
	return true;
}