using StoryTeller.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTeller.DataModel
{
    public class ProjectDataSource
    {
        private int _sceneNumberIncrement = 0;
        private const int LibraryItemsCount = 10;
        private const int ScenesCount = 10;
        public static StoryProject Project
        {
            get
            {
                return new ProjectDataSource().CreateStoryProject();
            }
        }

        private StoryProject CreateStoryProject()
        {
            Library lib = CreateLibrary();
            return new StoryProject
            {
                Library = lib,
                Story = CreateStoryProject(lib)
            };
        }

        private Story CreateStoryProject(Library lib)
        {
            IScene startScene = CreateScene(lib);
            IScene currentScene = startScene;
            for (int i = 0; i < ScenesCount; i++)
            {
                currentScene = currentScene.FollowingScene = CreateScene(lib);
            }

            return new Story
            {
                StartScene = startScene,
                Title = "Story of my life",
            };
        }

        private IScene CreateScene(Library lib)
        {
            List<LibraryItem> items = new List<LibraryItem>(lib.Items);
            LibraryItem item = items[new Random().Next(0, items.Count)];
            bool[] boolValues = new bool[] { true, false };
            Scene scene = new Scene(item)
            {
                IsBonusScene = boolValues[new Random().Next(0, boolValues.Length)],
            };

            return scene;
        }

        private Library CreateLibrary()
        {
            List<LibraryItem> items = new List<LibraryItem>();
            for (int i = 0; i < LibraryItemsCount; i++)
            {
                items.Add(CreateLibraryItem());
            }

            Library lib = new Library
            {
                Items = items
            };

            Library.Current = lib;

            return lib;
        }

        private LibraryItem CreateLibraryItem()
        {
            LibraryItem item = new LibraryItem()
            {
                SceneContent = CreateSceneContent()
            };

            return item;
        }

        private ISceneContent CreateSceneContent()
        {
            string links = "";
            for (int i = 0; i < LibraryItemsCount; i++)
            {
                links += string.Format("{{{0}:Scene#{0}}}, ", i.ToString());
            }
            
            return new TextSceneContent()
            {
                Content = @"Scene #" + (_sceneNumberIncrement++).ToString() + @"
" + links + @"

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent tristique ligula in ligula bibendum fermentum. Aenean auctor dui augue. Praesent molestie orci sit amet rhoncus elementum. Morbi auctor augue sed sodales maximus. Ut ornare gravida ultricies. Suspendisse dolor erat, tincidunt sit amet mattis vel, fermentum a diam. Aenean auctor nec arcu dapibus bibendum. Sed vitae diam mi. Morbi lacinia luctus sem vitae commodo.

Cras auctor laoreet tellus et varius. Integer vel faucibus turpis, et mattis dui. Nullam nec mi quam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Maecenas aliquam dui vel arcu euismod dictum. Nullam fringilla, sapien vel interdum tempus, felis sapien porttitor tellus, in euismod lacus felis ut ante. In blandit nibh felis, quis faucibus orci volutpat vel. Ut blandit tempus orci, a mattis ipsum sodales ut. Etiam sed lectus a quam commodo convallis quis vel augue. Quisque in lectus nibh. Etiam enim sem, placerat sit amet justo sed, pretium posuere mauris. Vivamus lacinia fringilla lectus. Nunc consectetur et nisi sit amet convallis. Vivamus lacinia, justo quis auctor sodales, nisl enim blandit turpis, sit amet laoreet mi lacus sed elit. Sed aliquet id nisl vehicula imperdiet.

Praesent eu pellentesque est. Phasellus dictum elit quis lectus fringilla posuere. Vivamus mi felis, vulputate id hendrerit vitae, luctus at nibh. Nulla rutrum id tellus aliquam porta. Vivamus pretium dolor ligula, eget porttitor dolor tincidunt et. Nullam mattis lorem quis ligula tincidunt, et feugiat odio ullamcorper. Donec eu porttitor orci. Quisque non diam condimentum, tincidunt nisi et, aliquet neque. Nullam rhoncus sodales odio, ac commodo augue tempus vitae. Nunc nisi tortor, vestibulum id blandit sed, fermentum ac sem. In ut neque at felis commodo tempor. Donec sit amet nibh nisl.

Aenean blandit justo erat. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Mauris tempor fringilla tempus. Quisque a ligula lacinia, efficitur quam non, egestas quam. Integer finibus ullamcorper malesuada. Morbi lacinia suscipit sapien eget lobortis. Morbi lectus neque, commodo at erat vitae, interdum feugiat dui. Ut congue sagittis faucibus. Cras vitae mauris nec ante feugiat vestibulum sit amet iaculis ipsum. Mauris gravida leo ut luctus condimentum. Phasellus sollicitudin neque id ultricies tincidunt. Integer id interdum dui, eget egestas erat. Nullam ornare augue eget leo tristique aliquam. Maecenas pharetra neque vel laoreet hendrerit. Donec quis sapien felis.

Nunc a viverra mi, non tristique urna. Nullam eros eros, finibus vel aliquam sed, volutpat id lacus. Ut facilisis, mauris sit amet cursus volutpat, lacus neque mollis velit, sed finibus lacus nisi sed nunc. Donec lacinia congue massa ut viverra. Integer nulla elit, ultricies non nisl quis, semper sodales urna. Duis ac sem eros. Ut sit amet ultricies purus. Nunc malesuada enim a cursus viverra. Curabitur lacinia mollis ex. Quisque nunc ex, congue laoreet ligula non, mollis feugiat turpis. Curabitur ut nisi in leo dapibus dapibus.

Vestibulum risus felis, finibus vel est pulvinar, tristique malesuada augue. Proin a finibus mi, aliquam maximus dui. Vivamus rutrum nulla in libero tincidunt, nec consequat dolor vulputate. Ut semper neque vitae libero sodales, a mollis magna viverra. In consectetur dapibus blandit. Proin ultricies massa sit amet ipsum sagittis, ut dignissim justo facilisis. Nam maximus porta mauris, quis vestibulum orci lobortis a. Nulla at posuere odio, et tristique diam. Integer sagittis volutpat est sed rhoncus. Donec ac nibh a lorem gravida consectetur.

Donec vel efficitur lorem. Sed laoreet ligula et feugiat ullamcorper. Fusce in nibh at nulla volutpat lacinia id in neque. Aliquam et pellentesque sem. Cras vehicula id lectus sed tincidunt. Ut volutpat dui ac iaculis condimentum. Sed nec sollicitudin eros. Donec nisl arcu, posuere et feugiat id, auctor sed quam. Nunc elementum, diam quis placerat iaculis, nisl purus varius urna, a tincidunt mauris dui eget quam. Interdum et malesuada fames ac ante ipsum primis in faucibus. Interdum et malesuada fames ac ante ipsum primis in faucibus. In dignissim nulla massa, a semper nibh tempus quis. Ut id ligula vitae dolor accumsan placerat. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.

Nam ac ullamcorper nisi. Morbi mattis rhoncus luctus. Pellentesque odio ligula, ornare vel magna eu, iaculis interdum urna. Curabitur pretium venenatis turpis eu blandit. Pellentesque tincidunt, massa a gravida suscipit, mi ligula tempus odio, ut suscipit mauris velit eu nunc. Sed enim dui, sodales et sapien a, vestibulum hendrerit mi. Nam porttitor vestibulum placerat. Aenean nunc tellus, euismod vitae nibh eu, luctus ullamcorper orci. Nam ullamcorper a est bibendum molestie. Phasellus aliquet a eros eu facilisis.

Curabitur at tincidunt arcu. Donec quis lectus id quam tincidunt gravida ac vel ipsum. Maecenas bibendum varius elit. Nam viverra fringilla sapien, ac consequat lorem facilisis in. In eget massa nisl. Vestibulum tristique dolor consequat dolor accumsan iaculis. Praesent ex sapien, accumsan eget nibh id, ultricies mollis augue. Donec tristique efficitur ante, vitae mollis magna vestibulum quis. Quisque lobortis non nunc aliquam ornare. Phasellus eros orci, iaculis ut pharetra et, dictum eu felis. Aliquam sollicitudin commodo imperdiet. Vivamus maximus dui in maximus volutpat. Cras non pharetra massa, eu fringilla nulla.

Maecenas gravida massa et eros fermentum consequat. Aliquam erat volutpat. In erat purus, mollis dictum mollis nec, ultrices vel libero. Sed ex diam, suscipit nec orci sit amet, viverra vestibulum magna. Duis consectetur bibendum ante nec faucibus. Praesent malesuada fermentum sapien in dictum. Phasellus efficitur libero eros, in faucibus lorem aliquam id. Ut porta maximus sollicitudin. Vivamus dictum dui a lorem malesuada imperdiet. Nulla eu posuere nisi, nec placerat mauris. Praesent varius arcu quis ultricies pulvinar. Maecenas non pharetra nunc, in pulvinar ex.

Maecenas fringilla vel lectus sit amet pretium. Maecenas molestie leo ex, elementum lobortis libero malesuada id. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed vehicula tellus in enim fermentum mollis. Mauris elementum malesuada elit, id bibendum ipsum pellentesque at. Nunc pulvinar vestibulum urna. Vestibulum vel efficitur augue. Nulla vel scelerisque velit, non rhoncus lectus. Aliquam a nulla libero. Suspendisse placerat libero non mi commodo, ac faucibus lorem efficitur. Morbi nulla elit, rutrum et elementum sed, condimentum eget lacus. Donec quis ultricies augue. Donec consequat luctus erat, nec elementum erat dapibus ac. Aenean tempus aliquam commodo. Nulla eget nisl rutrum, facilisis lorem eget, egestas felis.

Vivamus consequat justo a diam pulvinar, et faucibus nibh fermentum. In sed auctor urna. Ut semper a leo id efficitur. Praesent nec libero sit amet neque condimentum finibus. Vestibulum at ex fermentum, placerat diam vitae, commodo ante. Fusce efficitur fringilla ex vitae ornare. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In id urna nisl. Donec quam ligula, feugiat porttitor odio sit amet, tincidunt sollicitudin purus. Fusce dui enim, vulputate id pretium a, euismod nec mi. Fusce leo erat, cursus quis sodales at, efficitur et urna. Proin tempor, nibh sit amet viverra pulvinar, sem quam facilisis risus, non pellentesque augue enim a quam. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam sed ex rhoncus, tincidunt lorem eu, finibus magna. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Mauris neque urna, sodales quis facilisis et, malesuada vel diam.

Nam a erat quis risus consectetur tincidunt. Aenean nec porta mi. Nunc cursus accumsan auctor. Curabitur maximus magna vel neque mattis malesuada. Aenean quis gravida neque. Ut luctus vel dolor eu convallis. Sed laoreet, velit eget interdum viverra, odio elit posuere tellus, ut venenatis urna libero id felis. Pellentesque est lectus, aliquam non accumsan ullamcorper, aliquet ultricies ligula.

Vivamus varius convallis aliquet. Proin tempus dapibus dui nec ultrices. Nulla ultricies sapien quis mi sagittis, vel consectetur nulla volutpat. Curabitur rutrum fermentum mi vel elementum. Vestibulum augue quam, dictum vitae nulla a, hendrerit consectetur erat. In leo ipsum, placerat nec sodales in, sollicitudin quis urna. Aliquam erat volutpat.

Cras facilisis eleifend odio, ut rhoncus libero pharetra nec. Mauris non arcu in dui semper porttitor at vel ligula. Morbi dignissim, eros quis egestas eleifend, neque justo venenatis nibh, ac porttitor purus ante at leo. Morbi porttitor nisl non quam ullamcorper consectetur. Nunc porttitor scelerisque ipsum, a pretium nisl. In eu velit ut tellus maximus aliquet. Maecenas sem leo, porta eu ipsum volutpat, efficitur imperdiet risus. Maecenas feugiat porta ipsum eu accumsan. Nunc mattis eleifend tortor vel ullamcorper. Phasellus sed orci accumsan, lacinia dolor eget, aliquam ex. Suspendisse neque ligula, pretium feugiat maximus non, tincidunt ut elit. Pellentesque id nisl eu erat congue sagittis vel ut tortor. Donec eu massa tempus, convallis velit ut, pellentesque enim. In lacinia nunc id elementum lacinia.

In lacinia ante ac erat malesuada, id aliquam dolor tempus. Donec sagittis gravida dui. Suspendisse ornare bibendum turpis, quis interdum ligula lacinia eget. Praesent ultrices turpis a eros ullamcorper imperdiet. Vestibulum at quam consequat, auctor lectus eget, ullamcorper quam. In nec libero viverra risus ultrices eleifend in vitae libero. Maecenas sagittis ultricies elit id pulvinar.

Nulla facilisi. Maecenas dictum dui tellus, in posuere mauris gravida sit amet. Sed felis risus, bibendum at efficitur nec, consectetur quis erat. Vestibulum dignissim vehicula turpis, vel tristique nisi blandit ut. Ut molestie arcu leo, id auctor ex hendrerit et. Aenean quis sollicitudin nibh. Maecenas vitae massa euismod, euismod ante et, ornare urna.

Nulla dictum dignissim nisl, vitae sollicitudin nunc hendrerit vitae. Sed diam diam, lobortis a tortor ac, tincidunt cursus quam. Sed semper justo semper tristique semper. Donec dictum fringilla eros quis pretium. Nulla eleifend sem at malesuada dignissim. Cras vel sapien dolor. Fusce sed neque mi. Duis ut turpis at magna commodo dictum ac non libero. In hac habitasse platea dictumst. Vivamus tempor lectus sed diam luctus mattis in a erat. Fusce ac tempor nunc.

Nullam interdum cursus felis vel ultrices. Maecenas id varius lorem. Vivamus massa odio, laoreet ut justo eu, aliquam cursus ipsum. Etiam vestibulum consequat massa, laoreet imperdiet libero euismod pulvinar. Quisque luctus dictum mi sit amet tempor. Integer mauris magna, eleifend nec tincidunt non, lobortis eget nunc. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Morbi interdum feugiat ligula id malesuada. Aenean vel hendrerit justo, et condimentum libero. Maecenas blandit enim interdum volutpat bibendum. In ultricies hendrerit lorem, ut bibendum odio ultricies ac. Mauris nisl turpis, consectetur eget ligula sed, rutrum egestas arcu. Ut id consectetur nisl, nec malesuada nisi.

Sed ex nibh, fermentum sit amet volutpat accumsan, hendrerit a lectus. Integer facilisis tristique orci ac viverra. Suspendisse potenti. Mauris dapibus nunc nec quam pretium, ut placerat orci malesuada. In at nulla mauris. Quisque id tellus vitae felis fringilla tincidunt at ut tortor. Vivamus leo orci, euismod sed magna ut, tristique dignissim lectus. Nam consectetur luctus massa, quis tincidunt odio lobortis eget.

Vivamus sit amet egestas neque. Etiam ullamcorper, velit a sollicitudin rutrum, est lectus luctus nunc, ut volutpat libero diam quis metus. Fusce non orci vitae ex consectetur sagittis eu vel mi. In condimentum magna massa, sed tristique mi eleifend in. Praesent eu elementum urna. Proin facilisis arcu sed turpis rutrum, tristique auctor tortor luctus. Maecenas feugiat sit amet velit et porta. Proin libero leo, auctor nec mauris sit amet, scelerisque convallis diam. Donec eu accumsan tortor. Suspendisse consectetur semper ligula, sit amet suscipit turpis laoreet quis.

Quisque tincidunt, orci non semper aliquam, mauris magna accumsan arcu, ut dictum nisi lorem id orci. Mauris facilisis maximus blandit. Etiam semper tortor non sapien hendrerit iaculis. Curabitur vehicula quam erat, ac dictum leo efficitur vitae. Morbi vehicula laoreet ornare. Integer vel mi at lorem pulvinar faucibus. Sed commodo pulvinar lobortis. Pellentesque euismod iaculis lorem quis vulputate. Donec nisl tortor, ultricies in congue quis, tempor in ligula. Pellentesque facilisis volutpat urna id interdum. Aenean dapibus facilisis justo a dapibus.

Aenean magna diam, volutpat et risus a, venenatis venenatis ante. Pellentesque vestibulum orci nisl, a molestie odio porta eget. Aliquam erat volutpat. Fusce enim arcu, pellentesque tempus rhoncus a, malesuada eu arcu. Pellentesque consequat felis sed metus pretium eleifend. Nunc tincidunt viverra massa a euismod. Nulla dignissim porttitor sollicitudin. In hac habitasse platea dictumst. Cras placerat augue felis, non fringilla elit scelerisque elementum. Pellentesque sit amet orci vitae sapien molestie volutpat. Etiam porttitor, nisl vel dictum suscipit, sapien lorem viverra lorem, eget vestibulum nibh nisl ut ex. Nunc ultrices tempus turpis, vitae mattis diam.

Suspendisse sollicitudin neque eget leo faucibus, vel facilisis risus tempor. Nulla et viverra neque, vel molestie lectus. Sed cursus sapien eu diam cursus, vel dignissim nulla vestibulum. Ut hendrerit ipsum quam, quis accumsan mauris suscipit in. Sed vitae dignissim dolor, ac lobortis sapien. Donec ornare, ligula eleifend dignissim volutpat, velit tortor vestibulum libero, ut vestibulum purus purus a neque. Sed ipsum sapien, pellentesque at justo vitae, bibendum auctor eros. Integer rhoncus sed nulla a consectetur. Duis cursus dui tincidunt urna tincidunt cursus sed consectetur lacus. Aliquam quis fermentum turpis. In diam lorem, scelerisque quis lobortis nec, pulvinar nec arcu. Suspendisse sollicitudin nisl eget rhoncus condimentum. Fusce finibus gravida enim, ut varius tortor porttitor suscipit. Maecenas cursus ex eu euismod ultricies. Integer consequat viverra cursus.

Nulla fermentum mauris vitae urna convallis, sit amet suscipit nulla ullamcorper. Curabitur varius, magna non pulvinar lobortis, enim ex interdum felis, eget rhoncus nisi diam vel elit. Suspendisse sed facilisis odio. Nunc nec magna quis massa suscipit consequat. Vivamus augue diam, ullamcorper vel euismod non, iaculis ac diam. Ut rhoncus porta scelerisque. Duis auctor velit eu consectetur ultrices. Duis vulputate dapibus finibus. Quisque ut mattis justo, ac tempor ipsum.

Aenean pretium eu augue eu blandit. Mauris vitae iaculis leo. Pellentesque varius sapien eu turpis pellentesque, et varius est venenatis. Quisque iaculis volutpat tortor, at semper nisi faucibus sed. Nam hendrerit magna in odio aliquet, a semper urna auctor. Integer ultricies lacus urna, sed tempus nisi condimentum quis. Vestibulum eu justo nec ex bibendum sodales quis id risus.

Phasellus ultrices tortor mauris, in pellentesque libero interdum ut. Quisque sem ante, interdum vitae tortor a, congue rhoncus odio. Vestibulum magna erat, vestibulum in imperdiet ac, accumsan eget lacus. Aliquam eu sapien ex. Nam sed posuere enim. Vestibulum pharetra augue enim, at auctor lacus venenatis sit amet. Quisque id efficitur est. Mauris eu orci at nisi feugiat eleifend. In et ligula quis purus semper hendrerit nec eget est. Vivamus et auctor nibh, sed dictum urna. Nam varius, augue nec porta condimentum, ligula mi efficitur arcu, a varius nibh nibh quis ligula. Suspendisse nec porttitor augue, varius gravida ligula. Duis arcu sem, pellentesque eget iaculis ac, varius et metus.

Quisque ultricies felis sapien. Pellentesque dignissim nec enim vel convallis. Curabitur dapibus, libero a porta volutpat, turpis nulla feugiat turpis, a mollis est augue sed erat. Nam quis nisl vitae lorem vulputate ultrices at eu justo. Curabitur id urna faucibus, maximus augue id, lacinia velit. Vivamus nec lorem nunc. Aliquam commodo non odio at elementum. Quisque sit amet cursus nibh, a ullamcorper ligula. Nullam vel lorem non elit lobortis convallis.

Morbi molestie justo sit amet leo rutrum, sed dictum sem pellentesque. Morbi nec orci ultricies, scelerisque ipsum eu, lacinia turpis. Cras eget augue ornare, malesuada odio tincidunt, vulputate velit. Mauris et sem dolor. Praesent lobortis mauris libero, a scelerisque ligula ultrices vitae. Maecenas quis interdum lectus. Quisque eu ex tortor.

Pellentesque cursus facilisis enim, eu viverra nisl congue id. Vivamus ac enim lacinia, gravida elit eu, auctor mauris. Donec finibus, nibh ut sollicitudin euismod, nibh leo rhoncus mi, vel consectetur turpis tortor id tellus. Nunc arcu dui, placerat non sodales vel, egestas vel ipsum. Etiam id ullamcorper tellus, ac eleifend lorem. Curabitur dapibus nibh ullamcorper mollis consectetur. Duis dictum aliquet sodales. Nunc dictum blandit lorem, id ultricies elit mattis aliquam. Sed varius justo quis eros convallis dignissim.

Nunc aliquam imperdiet lacus quis gravida. Vestibulum arcu ante, pharetra id cursus eu, placerat at nibh. Aliquam eu lorem aliquet, vestibulum nunc vitae, ultricies sapien. Nulla a neque mattis risus venenatis eleifend. Morbi a risus non est luctus tincidunt. Nunc elementum eleifend volutpat. In eget urna malesuada, facilisis augue ut, tristique urna. Cras sed est nec enim pharetra tincidunt vel at ipsum. Fusce eu dictum libero, in mollis orci. Suspendisse metus enim, tristique in tempus id, iaculis nec erat. Cras vehicula metus ac maximus luctus. Phasellus vitae auctor odio. Ut iaculis sem sit amet velit mattis iaculis. Duis rutrum congue mauris non lobortis.

Pellentesque cursus sit amet massa sed sagittis. Proin eget dolor non sem rhoncus dictum. Vestibulum iaculis malesuada molestie. Nunc molestie imperdiet turpis, a lacinia sem condimentum non. Praesent dignissim sagittis vehicula. Aenean pellentesque commodo libero sit amet accumsan. Praesent ac nulla at est volutpat posuere at eget lectus. Nam fermentum tortor id erat aliquet, malesuada vehicula ante lobortis. Etiam non mauris a lectus egestas ultricies. Duis laoreet placerat arcu, ut ullamcorper purus faucibus eleifend. Curabitur quam nisi, vehicula vel euismod sit amet, posuere maximus mauris. Sed id elementum quam. Duis leo tellus, placerat non pharetra eget, tempor vel massa. Ut tincidunt felis ex, ut venenatis libero semper at. Integer tempus lorem ut nulla elementum, at varius libero consectetur.

Cras in ullamcorper nibh. Ut eget diam sem. Maecenas non aliquet enim, sit amet consectetur nibh. Maecenas interdum elit sit amet egestas finibus. Sed quis malesuada ipsum, eget tempor mauris. Sed eu mollis ligula. Etiam consequat, libero sed aliquet venenatis, metus ante vulputate nibh, at dictum ligula lectus sed arcu. Nulla id placerat sem. Cras ullamcorper enim vitae augue molestie rhoncus. Cras ipsum sapien, malesuada at justo eu, mattis viverra arcu. Vivamus lacinia odio id facilisis feugiat. Phasellus sed pretium justo, non consequat ligula. Cras a tempor ex. Etiam in ornare leo, eu faucibus eros. Donec non pretium nunc.

Donec vestibulum nibh id ligula tristique, vel facilisis urna hendrerit. Fusce ut eros et justo tempor dapibus ut vel lacus. Pellentesque facilisis tellus a felis volutpat auctor. Vivamus rhoncus, erat tincidunt semper viverra, arcu mauris feugiat tortor, at molestie diam nisl ac dolor. Praesent egestas sit amet justo vitae varius. Mauris in hendrerit mauris, vel gravida enim. Sed accumsan ornare dolor, sit amet elementum dui condimentum nec.

Nullam consequat, nibh et egestas mattis, libero tellus ultrices dui, eget convallis dui risus ac ex. Ut id luctus mauris. Integer consequat, velit eget mollis lacinia, elit erat mattis lectus, sed condimentum turpis nunc id dolor. Aliquam arcu est, scelerisque id massa non, dapibus venenatis ante. Ut porta enim id nisl luctus, a feugiat massa suscipit. Sed eget maximus sapien. Sed ut metus pharetra, cursus sem sed, sagittis diam. Aliquam scelerisque tempor eleifend. Donec pellentesque pharetra tincidunt. Nam ornare dignissim diam, vel commodo nunc fermentum et. Morbi quis massa in turpis convallis volutpat eget nec sapien. Maecenas et libero vulputate, elementum diam et, ornare ante. Praesent nisi erat, consequat id neque a, semper laoreet diam. Nunc sed turpis ullamcorper, dignissim odio eget, tincidunt tortor. Mauris vestibulum nunc a quam bibendum iaculis. Fusce dolor dui, porta porta libero at, semper vehicula velit.

Phasellus ac facilisis leo, eu efficitur libero. Nam volutpat rutrum dui, eu euismod elit elementum ut. Etiam egestas ex a orci tincidunt, et fringilla lorem facilisis. Ut laoreet enim at mi maximus luctus. Sed ullamcorper felis ut tortor sagittis, eget aliquet lectus laoreet. Proin pharetra, felis in vulputate condimentum, eros justo tristique neque, sit amet posuere velit tortor quis metus. Pellentesque pulvinar sem sed massa pellentesque finibus. Curabitur elit erat, dignissim a venenatis sed, auctor sed leo. Suspendisse vitae congue nulla. Fusce placerat mattis risus a sollicitudin. Pellentesque laoreet lacus lacus, scelerisque porta turpis pharetra sed.

Cras quis malesuada mauris. Ut id suscipit turpis, a facilisis nisl. Integer pellentesque lorem a nisi mollis rhoncus. Vestibulum eu ipsum et velit mattis auctor vel vitae massa. Phasellus posuere nulla ac turpis pretium, non aliquam libero sollicitudin. Suspendisse rhoncus, leo vitae vulputate congue, tellus enim condimentum dolor, id porttitor nibh augue at nulla. Ut leo justo, sagittis a lectus vel, efficitur accumsan elit.

Donec et laoreet sem, a eleifend turpis. Pellentesque vitae metus metus. Quisque posuere leo eu erat tristique vehicula. Donec eu nisl tincidunt metus lacinia pretium et in augue. Nulla ut dictum odio. Nunc ultrices mauris eu felis malesuada malesuada. Duis sed placerat sapien. Phasellus fringilla interdum molestie. Interdum et malesuada fames ac ante ipsum primis in faucibus. Nulla iaculis viverra magna nec viverra. Nam semper, velit ut vulputate tincidunt, felis velit aliquam mi, sit amet mollis nulla turpis vitae ante. Aenean ut fermentum odio, quis fringilla ante. Nullam convallis posuere ex, non rutrum est semper consectetur.

Nullam auctor ultrices nulla, sed pulvinar tellus aliquet eu. Pellentesque fringilla magna quam, id fermentum ligula tempus id. Sed consequat tempor aliquam. Suspendisse sed enim eros. Sed vel suscipit leo. Nunc sit amet pretium diam. Quisque laoreet mattis justo, non lobortis elit accumsan vitae. Cras iaculis enim leo. Ut a ornare neque. Aliquam erat volutpat. Suspendisse sed nisl rutrum, scelerisque odio nec, faucibus quam. Cras ultricies, diam vel viverra luctus, sapien turpis mattis sem, at rutrum turpis lorem ornare neque. Quisque nec mauris ac ante faucibus fermentum et sed lectus.

Curabitur sodales eu dui quis tincidunt. In hac habitasse platea dictumst. Vivamus venenatis a mauris sit amet bibendum. Sed sem velit, malesuada ut tellus in, malesuada venenatis velit. Quisque egestas elit lectus, vel elementum metus mattis quis. Vivamus nec maximus nunc. Cras non odio justo. Nullam et ligula imperdiet, sodales nisl eu, efficitur elit. Cras porttitor nunc eget nisl convallis egestas. Vestibulum congue nisi elit, et pharetra erat ultricies ac. Quisque dictum condimentum neque. Vestibulum lacinia velit non semper imperdiet. Aliquam sit amet leo nisl. Morbi congue, est sit amet cursus vulputate, dui neque gravida ligula, in tristique dolor elit quis sapien.

Ut dignissim odio sit amet dui consequat facilisis. Quisque laoreet nisi maximus, lobortis dolor a, tempus diam. In elementum consectetur nibh, et dictum turpis pharetra sit amet. Pellentesque venenatis est pretium, pharetra lorem vel, interdum metus. Nunc elementum quis nisi ac suscipit. Integer ornare interdum sodales. Nulla ipsum dolor, congue sit amet metus et, viverra convallis purus. Phasellus vitae urna nibh. Integer feugiat commodo libero, id tincidunt purus gravida id. Sed pretium, nibh quis varius ornare, dolor ante commodo orci, vitae tincidunt dolor magna et quam. Praesent eu augue arcu. Mauris sed odio non ex laoreet ullamcorper. Pellentesque semper, nibh nec condimentum lacinia, massa ante feugiat lacus, vestibulum aliquet nisl ligula quis dui.

Curabitur justo elit, mollis non fringilla a, pharetra non lorem. Phasellus pulvinar metus id tortor imperdiet, eget tincidunt urna semper. Phasellus nec mollis sem. Phasellus et nulla vitae erat aliquet volutpat. Aliquam nec lectus diam. In porta dapibus ornare. Praesent a mattis orci.

Sed lacus elit, porta quis elementum a, rhoncus sed risus. Suspendisse nec mi id dui lobortis pellentesque in sit amet nibh. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse fringilla accumsan eros, a cursus nibh tincidunt quis. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur dictum tellus eu accumsan commodo. Duis vel tortor ligula. Morbi efficitur interdum tempus. Vivamus mollis dui erat, a luctus lacus ornare sodales. Interdum et malesuada fames ac ante ipsum primis in faucibus. Etiam varius efficitur purus nec rutrum. Sed urna nisl, faucibus et ipsum sed, facilisis vestibulum ipsum.

Sed ultricies placerat quam a condimentum. Donec diam ante, ornare eget posuere eget, blandit in mauris. Nulla ac ultrices ipsum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi posuere bibendum lacus, ac pretium purus hendrerit in. Donec molestie, odio vitae imperdiet volutpat, metus massa pretium nunc, eget lobortis velit mauris id mauris. Cras condimentum gravida lectus, aliquet consectetur orci euismod vitae. Cras vel leo tellus. Donec eu ligula eget turpis eleifend mollis. Vivamus lacinia faucibus urna, sit amet pellentesque odio suscipit nec. Nullam imperdiet est enim, vitae tempor felis commodo ac. Phasellus sit amet nisi quis massa viverra mollis. Sed tellus odio, elementum eu placerat sit amet, pellentesque et orci. Sed blandit, elit sed pretium bibendum, purus magna congue lectus, bibendum condimentum nibh diam vitae arcu. Mauris non urna consequat diam maximus sodales. Maecenas vitae eros ligula.

Vestibulum mattis, augue eget pellentesque convallis, diam massa vulputate ante, vel laoreet risus felis a mi. Vivamus molestie efficitur metus ac dictum. Etiam a magna imperdiet, varius urna vel, eleifend turpis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Duis non egestas risus, ut maximus lectus. Quisque non justo dui. Aliquam vitae convallis lectus. Integer pharetra lectus sit amet volutpat sollicitudin. Donec efficitur risus eget augue dignissim, eu ornare metus suscipit. Phasellus egestas, quam et blandit condimentum, leo nisl cursus nibh, eget cursus ligula nulla quis urna. Aliquam et quam pretium, vestibulum augue ac, consectetur justo. Sed porttitor augue in nulla dapibus bibendum. Donec viverra mi massa, eget iaculis velit pharetra nec. Suspendisse id suscipit urna.

Quisque elementum elit sed maximus ornare. Integer porta magna eget vehicula condimentum. Curabitur rhoncus congue condimentum. Sed tempor justo sit amet mi tempus, nec finibus dui convallis. Mauris aliquam felis et consectetur feugiat. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In non arcu vel augue venenatis tincidunt non vel magna. Etiam ornare nec orci eget porttitor. Donec faucibus mi diam, id maximus nisi fermentum a. Duis imperdiet, elit et condimentum varius, nisl lorem vestibulum neque, quis aliquet leo nisi eget nisi. Nam efficitur volutpat orci sed auctor. Mauris varius nisl vel ex vehicula, at sollicitudin lectus molestie.

Proin varius diam ac dui dapibus, quis ultrices sapien pretium. Maecenas dignissim augue ut sem placerat, eu feugiat diam sollicitudin. Ut sit amet orci dolor. Ut felis risus, aliquam eget eleifend quis, ultrices auctor lacus. Morbi interdum, enim eu malesuada euismod, tortor nisl hendrerit lectus, id interdum lorem ipsum non metus. Nulla odio orci, ornare a odio id, maximus lobortis tortor. Donec id egestas erat. Mauris sodales laoreet elit sed scelerisque. Curabitur ac quam non nibh dignissim porttitor. Etiam vulputate urna nibh, a convallis libero luctus ac. Proin a libero mollis neque accumsan posuere. Ut libero elit, facilisis et arcu eget, sodales blandit metus. Nullam congue pretium tincidunt.

Vestibulum quis volutpat ipsum, vel elementum odio. Vivamus eget varius justo. Praesent id tortor arcu. Aliquam non metus vitae ligula semper elementum. Vestibulum ornare vitae odio vel ultrices. Cras ac risus est. Morbi convallis lacinia risus quis feugiat. Morbi justo massa, convallis sed laoreet in, rhoncus nec nisi. Ut molestie, mauris sit amet condimentum ultrices, ipsum eros vulputate tortor, eu molestie diam lorem vitae lorem.

Integer quis dolor sed tellus rutrum sagittis id et elit. Aenean bibendum orci augue, dignissim imperdiet lorem dapibus eu. Etiam porta leo id nibh porttitor egestas. Cras nec scelerisque enim, nec ultricies eros. Donec lectus lacus, molestie sit amet nulla sit amet, luctus finibus mauris. Quisque vel leo eget ex blandit sollicitudin. Phasellus tristique metus nisi, id porttitor enim dapibus quis. Nullam sit amet accumsan purus. Sed a vulputate justo, in efficitur ante. Sed sed euismod massa. Nulla id nulla ultrices, condimentum arcu sed, placerat metus. Sed turpis purus, imperdiet sed elit id, maximus tempus nibh. Donec vitae facilisis neque.

Cras id finibus urna. Proin ac augue ultricies, ullamcorper urna sit amet, porta nisl. Vestibulum laoreet velit sapien, eu ultrices diam feugiat a. Aenean at sodales mi. Aliquam lobortis molestie convallis. Suspendisse potenti. Quisque laoreet arcu leo, quis hendrerit purus consectetur vel. Donec tempus tempus lectus et finibus.

Generated 50 paragraphs, 4476 words, 30117 bytes of Lorem Ipsum

#END",
            };
        }
    }
}
