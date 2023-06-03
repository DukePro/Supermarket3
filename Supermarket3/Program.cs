namespace Supermarket
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.ShowMenu();
        }
    }

    class Menu
    {
        public void ShowMenu()
        {
            const string MenuServeClient = "1";
            const string MenuCreateClientsQueue = "2";
            const string MenuExit = "0";

            bool isExit = false;
            string userInput;
            Supermarket supermarket = new Supermarket();

            while (isExit == false)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuServeClient + " - Обслужить клиента");
                Console.WriteLine(MenuCreateClientsQueue + " - Создать очередь клиентов");
                Console.WriteLine(MenuExit + " - Выход");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuServeClient:
                        supermarket.ServeClient();
                        break;

                    case MenuCreateClientsQueue:
                        supermarket.CreateClients();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }
    }

    class Supermarket
    {
        private int _account;
        private Queue<Client> _clients = new Queue<Client>();

        public Supermarket()
        {
            CreateProducts();
        }

        public void CreateClients()
        {
            int clientsCount = 10;
            int clientsEntered = 0;

            for (int i = 0; i < clientsCount; i++)
            {
                Client client = new Client();
                client.FillShoppingCart(CreateProducts());
                _clients.Enqueue(client);
                clientsEntered++;
            }

            Console.WriteLine($"В очередь встало {clientsEntered} клиентов");
            Console.WriteLine($"Всего клиентов в очереди - {_clients.Count()}");
        }

        public void ServeClient()
        {
            if (_clients.Count > 0)
            {
                bool isPayed = false;
                Client client = _clients.Peek();

                while (isPayed == false)
                {
                    int totalPrice = client.CalculateCartPrice();

                    if (client.Money >= totalPrice)
                    {
                        _account += client.Pay(totalPrice);
                        _clients.Dequeue();

                        if (totalPrice > 0)
                        {
                            Console.WriteLine($"Товары на сумму {totalPrice} оплачены, клиент обслужен.");
                        }
                        else
                        {
                            Console.WriteLine("Клиент ничего не купил");
                        }

                        Console.WriteLine($"Балланс магазина - {_account}");
                        Console.WriteLine($"Клиентов в очереди - {_clients.Count()}");
                        isPayed = true;
                    }
                    else
                    {
                        client.RemoveFromCart();
                    }
                }
            }
            else
            {
                Console.WriteLine("Очередь клиентов пуста");
            }
        }

        private Product[] CreateProducts()
        {
            return new Product[]
        {
            new Product("Хлеб", 50),
            new Product("Молоко", 70),
            new Product("Яйца", 40),
            new Product("картошка", 45),
            new Product("Сметана", 70),
            new Product("Булка", 40),
            new Product("Жир", 20),
            new Product("Пиво", 140),
            new Product("Куриный пупок", 40),
            new Product("Кислый пупс", 100),
            new Product("Пюрешка", 70),
            new Product("Салат ВсемРад", 110),
            new Product("Салат ТухлыйСмрад", 20),
            new Product("Салат ВесёлыйВлад", 120),
            new Product("Мясо Птицы", 80),
            new Product("Мясо Рыбы", 70),
            new Product("Ни рыба - ни мясо", 60),
            new Product("Колбаса копчёная", 130),
            new Product("Колбаса варёная", 135),
            new Product("Цыплёнок жареный", 110),
            new Product("Цыплёнок пареный", 115),
            new Product("Чай", 80),
            new Product("Кофе", 70),
            new Product("Потанцуем", 145)
        };
        }
    }

    class Client
    {
        private Random _random = new Random();
        private ShoppingCart _shoppingCart = new ShoppingCart();

        public Client()
        {
            Money = CreateMoney();
        }

        public int Money { get; private set; }

        public void FillShoppingCart(Product[] products)
        {
            int minproductsToBuy = 0;
            int maxProductsToBuy = 10;
            int productsToBuy = _random.Next(minproductsToBuy, maxProductsToBuy);

            for (int i = 0; i < productsToBuy; i++)
            {
                _shoppingCart.AddProduct(products[_random.Next(0, products.Length)]);
            }
        }

        public int CalculateCartPrice()
        {
            Product[] products = _shoppingCart.GetProducts();
            int totalPrice = 0;

            if (products.Length > 0)
            {
                for (int i = 0; i < products.Length; i++)
                {
                    totalPrice += products[i].Price;
                }

                return totalPrice;
            }
            else
            {
                Console.WriteLine("В корзине пусто");
                return totalPrice;
            }
        }

        public void RemoveFromCart()
        {
            Product[] products = _shoppingCart.GetProducts();
            int itemIndex = _random.Next(0, products.Length);
            Console.WriteLine($"У клиента не хватило денег, товар - \"{products[itemIndex].Name}\" выложен из корзины");
            _shoppingCart.RemoveProduct(products[itemIndex]);
        }

        public int Pay(int price)
        {
            if (Money >= price)
            {
                Money -= price;
                return price;
            }
            else
            {
                Console.WriteLine("Не хватает денег");
                return 0;
            }
        }

        private int CreateMoney()
        {
            int minMoney = 200;
            int maxMoney = 350;

            return _random.Next(minMoney, maxMoney);
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }
    }

    class ShoppingCart
    {
        private List<Product> _shoppingCart;

        public ShoppingCart()
        {
            _shoppingCart = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            _shoppingCart.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            _shoppingCart.Remove(product);
        }

        public Product[] GetProducts()
        {
            return _shoppingCart.ToArray();
        }
    }
}