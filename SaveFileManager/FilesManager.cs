namespace SaveFileManager
{
    public static class FilesManager
    {
        /// <summary>
        /// Allows the user to pick between creating a new save, loading an old save and deleteing a save.<br/>
        /// Reads in file data from a <c>FileReader</c> method.<br/>
        /// Returns a tuple depending on what the user selected, containig what happenend, and in whitch slot.<br/>
        /// Can return: new, load, delete
        /// </summary>
        /// <param name="filesData">The return value from a <c>FileReader</c> method.</param>
        /// <param name="maxFiles">The maximum number of files that can exist. If the number of files go abowe this number, no new files can be created. -1 for no limit.</param>
        /// <param name="fileName">The name of the files without the extension. The a "*"-s in the name will be replaced with the file number.</param>
        /// <param name="fileExt">The extension of the files.</param>
        /// <returns></returns>
        public static (FileManagerOptions ManagerOption, int slotNumber) ManageFiles(Dictionary<string, List<string>?> filesData, int maxFiles = -1, string fileName = "file*", string fileExt = "sav")
        {
            var option = 1;
            var manageExit = false;
            while (!manageExit)
            {
                if (filesData.Count() > 0)
                {
                    // get file range
                    var maxFileNum = 0;
                    var minFileNum = int.MaxValue;
                    foreach (var fileNumStr in filesData.Keys)
                    {
                        if (int.TryParse(fileNumStr, out int fileNum))
                        {
                            if (fileNum > maxFileNum)
                            {
                                maxFileNum = fileNum;
                            }
                            if (fileNum < minFileNum)
                            {
                                minFileNum = fileNum;
                            }
                        }
                    }
                    option = Utils.ReadInt($"Select an option: -1: delete mode, 0: new file, {minFileNum}-{maxFileNum}: load file: ");
                    // delete
                    if (option == -1)
                    {
                        option = Utils.ReadInt($"Select an option: 0: back, {minFileNum}-{maxFileNum}: delete file: ");
                        if (filesData.ContainsKey(option.ToString()))
                        {
                            var sure = Utils.Input($"Are you sure you want to remove Save file {option}?(Y/N): ");
                            if (sure is not null && sure.ToUpper() == "Y")
                            {
                                File.Delete($"{fileName.Replace("*", option.ToString())}.{fileExt}");
                                manageExit = true;
                            }
                        }
                        else if (option != 0)
                        {
                            Console.WriteLine($"Save file {option} doesn't exist!");
                        }
                    }
                    // new file
                    else if (option == 0)
                    {
                        var newSlot = 1;
                        foreach (var fileNumStr in filesData.Keys)
                        {
                            if (int.TryParse(fileNumStr, out int fileNum) && fileNum == newSlot)
                            {
                                newSlot++;
                            }
                        }
                        if (maxFiles < 0 || newSlot <= maxFiles)
                        {
                            return (FileManagerOptions.NEW_FILE, newSlot);
                        }
                        else
                        {
                            Utils.PressKey("No empty save files! Delete a file to continue!");
                        }
                    }
                    // load
                    else
                    {
                        if (filesData.ContainsKey(option.ToString()))
                        {
                            return (FileManagerOptions.LOAD_FILE, option);
                        }
                        Console.WriteLine($"Save file {option} doesn't exist!");
                    }
                }
                else
                {
                    Utils.PressKey("No save files!");
                    return (FileManagerOptions.NEW_FILE, 1);
                }
            }
            return (FileManagerOptions.DELETE_FILE, option);
        }

        /// <summary>
        /// Allows the user to pick between creating a new save, loading an old save and deleteing a save, with UI selection.<br/>
        /// Reads in file data from a <c>FileReader</c> method.<br/>
        /// Returns a tuple depending on what the user selected, containig what happenend, and in whitch slot (except exit).<br/>
        /// Can return: new, load, exit
        /// </summary>
        /// <param name="filesData">The return value from a <c>FileReader</c> method.</param>
        /// <param name="maxFiles">The maximum number of files that can exist. If the number of files go abowe this number, no new files can be created. -1 for no limit.</param>
        /// <param name="fileName">The name of the files without the extension. The a "*"-s in the name will be replaced with the file number.</param>
        /// <param name="fileExt">The extension of the files.</param>
        /// <param name="canExit">If the user can exit from this menu with he key assigned to the escape action.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use.</param>
        /// <returns></returns>
        //    public (FileManagerOptions ManagerOption, int slotNumber) ManageFilesUI(Dictionary<string, List<string>?> filesData, int maxFiles = -1, string fileName = "file*", string fileExt = "sav", bool canExit = false, IEnumerable<KeyAction>? keybinds = null)
        //    {
        //        in_main_menu = True
        //        while True:
        //            if len(file_data) :
        //                if in_main_menu:
        //                    in_main_menu = False
        //                    option = UI_list(["New save", "Load/Delete save"], " Main menu", can_esc = can_exit).display(keybinds = keybinds)
        //                else:
        //                    option = 1
        //                // new file
        //                if option == 0:
        //                    new_slot = 1
        //                    for data in file_data:
        //                        if data[0] == new_slot:
        //                            new_slot += 1
        //                    if new_slot <= max_saves or max_saves< 0:
        //                        return (1, new_slot)
        //                    else:
        //                        input(f"No empty save files! Delete a file to continue!")
        //                elif option == -1:
        //                    return (-1, -1)
        //                // load / delete
        //                else:
        //                    // get data from file_data
        //                    list_data = []
        //                    for data in file_data:
        //                        list_data.append(f"{data[0]}. {data[1]}")
        //                    list_data.append(None)
        //                    list_data.append("Delete file")
        //                    list_data.append("Back")
        //                    option = UI_list(list_data, " Level select", can_esc = True).display(keybinds)
        //                    // load
        //                    if option != -1 and option<len(file_data):
        //                        return (0, file_data[option][0])
        //                    // delete
        //                    elif option == len(file_data) + 1:
        //                        list_data.pop(len(list_data) - 2)
        //                        delete_mode = True
        //                        while delete_mode and len(file_data) > 0:
        //                            option = UI_list(list_data, " Delete mode!", Cursor_icon("X ", "", "  "), multiline = False, can_esc = True).display(keybinds)
        //                            if option != -1 and option != len(list_data) - 1:
        //                                sure = UI_list(["No", "Yes"], f" Are you sure you want to remove Save file {file_data[option][0]}?", can_esc = True).display(keybinds)
        //                                if sure == 1:
        //                                    remove(f'{save_name.replace("*", str(file_data[option][0]))}.{save_ext}')
        //                                    list_data.pop(option)
        //                                    file_data.pop(option)
        //                            else:
        //                                delete_mode = False
        //                    // back
        //                    else:
        //                        in_main_menu = True
        //            else:
        //                input(f"\n No save files detected!")
        //                return (1, 1)
        //    }


        //def manage_saves_ui_2(new_save_function:list[Callable | Any], load_save_function:list[Callable | Any], get_data_function:list[Callable | Any]|None= None, max_saves= -1, save_name= "save*", save_ext= "sav", can_exit= False, keybinds:Keybinds|None= None):
        //    """
        //    Allows the user to pick between creating a new save, loading an old save and deleteing a save, with UI selection.\n
        //    The new_save_function and the load_save_function run, when the user preforms these actions, and both WILL get the file number, that was refrenced as their first argument.\n
        //    The get_data_function should return a list with all of the save file data, similar to the file_redaer function.\n
        //    The first element of all function lists should allways be the function. All other elements will be treated as arguments for that function.
        //    """

        //    # get_fuction default
        //    if get_data_function is None:
        //        get_data_function = [file_reader, max_saves, save_name, save_ext]

        //    def new_save_pre(new_func):
        //        if not callable(get_data_function[0]):
        //            get_data_function[0] = file_reader
        //        file_data = get_data_function[0](*get_data_function[1:])
        //        new_slot = 1
        //        for data in file_data:
        //            if data[0] == new_slot:
        //                new_slot += 1
        //        if new_slot <= max_saves or max_saves< 0:
        //            new_func[0](new_slot, * new_func[1:])
        //        else:
        //            input(f"No empty save files! Delete a file to continue!")

        //    def load_or_delete(load_func):
        //        while True:
        //            # get data from file_data
        //            if not callable(get_data_function[0]):
        //                get_data_function[0] = file_reader
        //            file_data = get_data_function[0](*get_data_function[1:])
        //            list_data = []
        //            for data in file_data:
        //                list_data.append(f"{data[0]}. {data[1]}")
        //                list_data.append(None)
        //            list_data.append("Delete file")
        //            list_data.append("Back")
        //            option = UI_list(list_data, " Level select", can_esc= True).display(keybinds)
        //            # load
        //            if option != -1 and option / 2 < len(file_data) :
        //                load_func[0] (file_data[int(option / 2)][0], * load_func[1:])
        //            # delete
        //            elif option / 2 == len(file_data) :
        //                list_data.pop(len(list_data) - 2)
        //                delete_mode = True
        //                while delete_mode and len(file_data) > 0:
        //                    option = UI_list(list_data, " Delete mode!", Cursor_icon("X ", "", "  "), multiline=False, can_esc=True).display(keybinds)
        //                    if option != -1 and option != len(list_data) - 1:
        //                        option = int (option / 2)
        //                        sure = UI_list(["No", "Yes"], f" Are you sure you want to remove Save file {file_data[option][0]}?", can_esc= True).display(keybinds)
        //                        if sure == 1:
        //                            remove(f'{save_name.replace("*", str(file_data[option][0]))}.{save_ext}')
        //                            list_data.pop(option)
        //                            list_data.pop(option)
        //                            file_data.pop(option)
        //                    else:
        //                        delete_mode = False
        //                if len(file_data) == 0:
        //                    input(f"\n No save files detected!")
        //                    new_save_function[0] (1, *new_save_function[1:])
        //            else:
        //                break

        //    # actual function
        //    if not callable(get_data_function[0]):
        //        get_data_function[0] = file_reader
        //    file_data = get_data_function[0](*get_data_function[1:])
        //    # main
        //    if len(file_data) :
        //        option = UI_list(["New save", "Load/Delete save"], " Main menu", can_esc = can_exit, action_list = [[new_save_pre, new_save_function], [load_or_delete, load_save_function]]).display(keybinds)
        //        if option == -1:
        //            return -1
        //    else:
        //        input(f"\n No save files detected!")
        //        new_save_function[0] (1, *new_save_function[1:])
    }
}
